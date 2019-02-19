require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# Configuration
# --------------------------------------------------------------------------- #
SOLUTION       = 'Cube.Pdf'
SUFFIX         = 'Applications'
PROJECTS       = [ 'Core', 'Ghostscript', 'Itext', 'Pdfium' ]
BRANCHES       = [ 'stable', 'net35' ]
PLATFORMS      = [ 'x86', 'x64' ]
CONFIGURATIONS = [ 'Debug', 'Release' ]
NATIVE         = '../resources/native'
PDFIUM         = [ 'PdfiumViewer.Native', 'no_v8-no_xfa', '2018.4.8.256' ]
TESTCASES      = {
    'Cube.Pdf.Tests'            => 'Tests',
    'Cube.Pdf.Tests.Converter'  => 'Applications/Converter/Tests',
    'Cube.Pdf.Tests.Editor'     => 'Applications/Editor/Tests',
    'Cube.Pdf.Tests.Pinstaller' => 'Applications/Pinstaller/Tests'
}

# --------------------------------------------------------------------------- #
# Commands
# --------------------------------------------------------------------------- #
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'
TEST     = '../packages/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe'

# --------------------------------------------------------------------------- #
# Functions
# --------------------------------------------------------------------------- #
def do_copy(src, dest)
    FileUtils.mkdir_p(dest)
    FileUtils.cp_r(src, dest)
end

# --------------------------------------------------------------------------- #
# Tasks
# --------------------------------------------------------------------------- #
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:copy].execute
    Rake::Task[:pack].execute
end

# --------------------------------------------------------------------------- #
# Build
# --------------------------------------------------------------------------- #
task :build do
    BRANCHES.each { |branch|
        sh("#{CHECKOUT} #{branch}")
        sh("#{RESTORE} #{SOLUTION}.#{SUFFIX}.sln")
        sh("#{BUILD} #{SOLUTION}.#{SUFFIX}.sln")
    }
end

# --------------------------------------------------------------------------- #
# Pack
# --------------------------------------------------------------------------- #
task :pack do
    sh("#{CHECKOUT} net35")
    PROJECTS.each { |proj| sh("#{PACK} Libraries/#{proj}/#{SOLUTION}.#{proj}.nuspec") }
    sh("#{CHECKOUT} master")
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
task :copy do
    [ '', 'net35' ].product(PLATFORMS, CONFIGURATIONS) { |set|
        x86_64  = [ 'bin', set[0], set[1], set[2] ].compact.reject(&:empty?).join('/')
        any_cpu = [ 'bin', set[0], set[2] ].compact.reject(&:empty?).join('/')

        # Ghostscript
        [ 'Tests', 'Applications/Converter/Tests', 'Applications/Converter/Forms' ].each { |dest|
            src  = [ NATIVE, set[1], 'gs', 'gsdll32.dll' ].join('/')
            do_copy(src, "#{dest}/#{x86_64}")
            do_copy(src, "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }

        # PDFium
        [ 'Tests', 'Applications/Editor/Tests', 'Applications/Editor/Forms' ].each { |dest|
            arch = (set[1] == 'x86') ? 'x86' : 'x86_64'
            dir  = [ PDFIUM[0], arch, PDFIUM[1], PDFIUM[2] ].join('.')
            src  = [ '..', 'packages', dir, 'Build', set[1], 'pdfium.dll' ].join('/')
            do_copy(src, "#{dest}/#{x86_64}")
            do_copy(src, "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }
    }
end

# --------------------------------------------------------------------------- #
# Test
# --------------------------------------------------------------------------- #
task :test do
    sh("#{RESTORE} #{SOLUTION}.#{SUFFIX}.sln")
    sh("#{BUILD} #{SOLUTION}.#{SUFFIX}.sln")

    branch = `git symbolic-ref --short HEAD`.chomp
    TESTCASES.each { |proj, dir|
        src = branch == 'net35' ?
              "#{dir}/bin/net35/Release/#{proj}.dll" :
              "#{dir}/bin/Release/#{proj}.dll"
        sh("#{TEST} #{src}")
    }
end

# --------------------------------------------------------------------------- #
# Clean
# --------------------------------------------------------------------------- #
CLEAN.include(%w{exe dll nupkg log}.map{ |e| "**/*.#{e}" })