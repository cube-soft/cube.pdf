# --------------------------------------------------------------------------- #
#
# Copyright (c) 2010 CubeSoft, Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#  http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
# --------------------------------------------------------------------------- #
require 'rake'
require 'rake/clean'

# --------------------------------------------------------------------------- #
# configuration
# --------------------------------------------------------------------------- #
PROJECT     = 'Cube.Pdf'
APPLICATION = 'Applications'
LIBRARY     = '../packages'
NATIVE      = '../resources/native'
BRANCHES    = ['stable', 'net35']
FRAMEWORKS  = ['net45', 'net35']
CONFIGS     = ['Release', 'Debug']
PLATFORMS   = ['Any CPU', 'x86', 'x64']
PDFIUM      = [ 'PdfiumViewer.Native', 'no_v8-no_xfa', '2018.4.8.256' ]
PACKAGES    = [
    "Libraries/#{PROJECT}.Core.nuspec",
    "Libraries/#{PROJECT}.Ghostscript.nuspec",
    "Libraries/#{PROJECT}.Itext.nuspec",
    "Libraries/#{PROJECT}.Pdfium.nuspec"
]
TESTCASES   = {
    'Cube.Pdf.Tests'            => 'Tests',
    'Cube.Pdf.Tests.Converter'  => 'Applications/Converter/Tests',
    'Cube.Pdf.Tests.Editor'     => 'Applications/Editor/Tests',
    'Cube.Pdf.Tests.Pinstaller' => 'Applications/Pinstaller/Tests'
}

# --------------------------------------------------------------------------- #
# copy targets
# --------------------------------------------------------------------------- #
CP_GS       = ['Tests', 'Applications/Converter/Tests', 'Applications/Converter/Forms']
CP_PDFIUM   = ['Tests', 'Applications/Editor/Tests', 'Applications/Editor/Forms' ]

# --------------------------------------------------------------------------- #
# commands
# --------------------------------------------------------------------------- #
BUILD = "msbuild -v:m -t:build -p:Configuration=#{CONFIGS[0]}"
PACK  = %(nuget pack -Properties "Configuration=#{CONFIGS[0]};Platform=AnyCPU")
TEST  = "../packages/NUnit.ConsoleRunner/3.10.0/tools/nunit3-console.exe"

# --------------------------------------------------------------------------- #
# clean
# --------------------------------------------------------------------------- #
CLEAN.include("#{PROJECT}.*.nupkg")
CLEAN.include("#{LIBRARY}/cube.*")
CLEAN.include(['bin', 'obj'].map{ |e| "**/#{e}" })

# --------------------------------------------------------------------------- #
# default
# --------------------------------------------------------------------------- #
desc "Build the solution and create NuGet packages."
task :default => [:clean_build, :pack]

# --------------------------------------------------------------------------- #
# pack
# --------------------------------------------------------------------------- #
desc "Create NuGet packages in the net35 branch."
task :pack do
    sh("git checkout net35")
    PACKAGES.each { |e| sh("#{PACK} #{e}") }
    sh("git checkout master")
end

# --------------------------------------------------------------------------- #
# clean_build
# --------------------------------------------------------------------------- #
desc "Clean objects and build the solution in pre-defined branches and platforms."
task :clean_build => [:clean] do
    BRANCHES.product(['Any CPU']) { |e|
        sh("git checkout #{e[0]}")
        RakeFileUtils::rm_rf(FileList.new("#{LIBRARY}/cube.*"))
        Rake::Task[:build].reenable
        Rake::Task[:build].invoke(e[1])
    }
end

# --------------------------------------------------------------------------- #
# build
# --------------------------------------------------------------------------- #
desc "Build the solution in the current branch."
task :build, [:platform] do |_, e|
    e.with_defaults(:platform => PLATFORMS[0])
    sh("nuget restore #{PROJECT}.#{APPLICATION}.sln")
    sh(%(#{BUILD} -p:Platform="#{e.platform}" #{PROJECT}.#{APPLICATION}.sln))
end

# --------------------------------------------------------------------------- #
# test
# --------------------------------------------------------------------------- #
desc "Build and test projects in the current branch."
task :test => [:build] do
    fw  = `git symbolic-ref --short HEAD`.chomp
    fw  = 'net45' if (fw != 'net35')
    bin = ['bin', PLATFORMS[0], CONFIGS[0], fw].join('/')

    Rake::Task[:copy].reenable
    Rake::Task[:copy].invoke(fw)

    TESTCASES.each { |proj, root|
        dir = "#{root}/#{bin}"
        sh(%(#{TEST} "#{dir}/#{proj}.dll" --work="#{dir}"))
    }
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
desc "Copy resources to the bin directories."
task :copy, [:framework] do |_, e|
    fw = (e.framework != nil) ? [e.framework] : FRAMEWORKS
    fw.product(PLATFORMS, CONFIGS) { |set|
        pf  = (set[1] == 'Any CPU') ? 'x64' : set[1]
        bin = ['bin', set[1], set[2], set[0]].join('/')

        # Ghostscript
        CP_GS.each { |root|
            src  = "#{NATIVE}/#{pf}/gs/gsdll32.dll"
            dest = "#{root}/#{bin}"
            RakeFileUtils::mkdir_p(dest)
            RakeFileUtils::cp_r(src, dest)
        }

        # PDFium
        CP_PDFIUM.each { |root|
            arch = (set[1] == 'x86') ? 'x86' : 'x86_64'
            dir  = [PDFIUM[0], arch, PDFIUM[1], PDFIUM[2]].join('.')
            src  = [LIBRARY, dir, 'Build', set[1], 'pdfium.dll'].join('/')
            dest = "#{root}/#{bin}"
            RakeFileUtils::mkdir_p(dest)
            RakeFileUtils::cp_r(src, dest)
        }
    }
end
