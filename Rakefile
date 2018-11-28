require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# Configuration
# --------------------------------------------------------------------------- #
SOLUTION       = 'Cube.Pdf'
PROJECTS       = [ 'Core', 'Ghostscript', 'Itext', 'Pdfium' ]
BRANCHES       = [ 'master', 'net35' ]
PLATFORMS      = [ 'x86', 'x64' ]
CONFIGURATIONS = [ 'Debug', 'Release' ]
PACKAGE        = '../packages'
NATIVE         = '../resources/native'
PDFIUM         = [ 'PdfiumViewer.Native', 'no_v8-no_xfa', '2018.4.8.256' ]

# --------------------------------------------------------------------------- #
# Commands
# --------------------------------------------------------------------------- #
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'

# --------------------------------------------------------------------------- #
# Functions
# --------------------------------------------------------------------------- #
def fs_copy(src, dest)
    FileUtils.mkdir_p(dest)
    FileUtils.cp_r(Dir.glob(src), dest)
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
        sh("#{RESTORE} #{SOLUTION}.sln")
        sh("#{BUILD} #{SOLUTION}.sln")
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
            src  = [ NATIVE, set[1], 'ghostscript', 'gsdll32.dll' ].join('/')
            fs_copy(src, "#{dest}/#{x86_64}")
            fs_copy(src, "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }

        # PDFium
        [ 'Tests', 'Applications/Editor/Tests', 'Applications/Editor/Forms' ].each { |dest|
            arch = (set[1] == 'x86') ? 'x86' : 'x86_64'
            dir  = [ PDFIUM[0], arch, PDFIUM[1], PDFIUM[2] ].join('.')
            src  = [ PACKAGE, dir, 'Build', set[1], 'pdfium.dll' ].join('/')
            fs_copy(src, "#{dest}/#{x86_64}")
            fs_copy(src, "#{dest}/#{any_cpu}") if (set[1] == 'x64')
        }
    }
end

# --------------------------------------------------------------------------- #
# Clean
# --------------------------------------------------------------------------- #
CLEAN.include(%w{exe dll nupkg log}.map{ |e| "**/*.#{e}" })