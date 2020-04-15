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
PROJECT     = "Cube.Pdf"
MAIN        = "Cube.Pdf.Apps"
LIB         = "../packages"
BRANCHES    = ["master", "net35"]
FRAMEWORKS  = ["net45", "net35"]
CONFIGS     = ["Release", "Debug"]
PLATFORMS   = ["Any CPU", "x86", "x64"]
PACKAGES    = ["Libraries/Core/Cube.Pdf.Core.nuspec",
               "Libraries/Ghostscript/Cube.Pdf.Ghostscript.nuspec",
               "Libraries/Itext/Cube.Pdf.Itext.nuspec",
               "Libraries/Pdfium/Cube.Pdf.Pdfium.nuspec",
               "Applications/Converter/Core/Cube.Pdf.Converter.nuspec"]
TESTCASES   = {"Cube.Pdf.Tests"            => "Libraries/Tests",
               "Cube.Pdf.Converter.Tests"  => "Applications/Converter/Tests",
               "Cube.Pdf.Editor.Tests"     => "Applications/Editor/Tests",
               "Cube.Pdf.Pages.Tests"      => "Applications/Pages/Tests",
               "Cube.Pdf.Clip.Tests"       => "Applications/Clip/Tests",
               "Cube.Pdf.Picker.Tests"     => "Applications/Picker/Tests"}

# --------------------------------------------------------------------------- #
# unmanaged libraries
# --------------------------------------------------------------------------- #
COPIES = {
    "Cube.Native.Ghostscript/9.52.0" => [
        "Libraries/Tests",
        "Applications/Converter/Tests",
        "Applications/Converter/Main"
    ],
    "Cube.Native.Pdfium.Lite/1.0.4044" => [
        "Libraries/Tests",
        "Applications/Editor/Tests",
        "Applications/Editor/Main"
    ]
}

# --------------------------------------------------------------------------- #
# commands
# --------------------------------------------------------------------------- #
RESTORE = "dotnet restore"
BUILD   = "msbuild -v:m -p:Configuration=#{CONFIGS[0]}"
TEST    = "../packages/NUnit.ConsoleRunner/3.10.0/tools/nunit3-console.exe"
PACK    = %(nuget pack -Properties "Configuration=#{CONFIGS[0]};Platform=AnyCPU")

# --------------------------------------------------------------------------- #
# clean
# --------------------------------------------------------------------------- #
CLEAN.include(["bin", "obj"].map{ |e| "**/#{e}" })
CLEAN.include("#{PROJECT}.*.nupkg")
CLOBBER.include("#{LIB}/cube.*")

# --------------------------------------------------------------------------- #
# default
# --------------------------------------------------------------------------- #
desc "Clean, build, and create NuGet packages."
task :default => [:clean, :build_all, :pack]

# --------------------------------------------------------------------------- #
# pack
# --------------------------------------------------------------------------- #
desc "Create NuGet packages in the net35 branch."
task :pack do
    checkout("net35") { PACKAGES.each { |e| cmd("#{PACK} #{e}") }}
end

# --------------------------------------------------------------------------- #
# restore
# --------------------------------------------------------------------------- #
desc "Resote NuGet packages in the current branch."
task :restore do
    cmd("#{RESTORE} #{MAIN}.sln")
end

# --------------------------------------------------------------------------- #
# build
# --------------------------------------------------------------------------- #
desc "Build projects in the current branch."
task :build, [:platform] do |_, e|
    e.with_defaults(:platform => PLATFORMS[0])
    Rake::Task[:restore].execute
    cmd(%(#{BUILD} -p:Platform="#{e.platform}" #{MAIN}.sln))
end

# --------------------------------------------------------------------------- #
# clean_build
# --------------------------------------------------------------------------- #
desc "Build projects in pre-defined branches and platforms."
task :build_all do
    BRANCHES.product(PLATFORMS) { |set|
        checkout(set[0]) do
            Rake::Task[:build].reenable
            Rake::Task[:build].invoke(set[1])
        end
    }
end

# --------------------------------------------------------------------------- #
# build_test
# --------------------------------------------------------------------------- #
desc "Build and test projects in the current branch."
task :build_test => [:build, :test]

# --------------------------------------------------------------------------- #
# test
# --------------------------------------------------------------------------- #
desc "Build and test projects in the current branch."
task :test do
    fw  = %x(git symbolic-ref --short HEAD).chomp
    fw  = 'net45' if (fw != 'net35')
    bin = ['bin', PLATFORMS[0], CONFIGS[0], fw].join('/')
    TESTCASES.each { |p, d| sh(%(#{TEST} "#{d}/#{bin}/#{p}.dll" --work="#{d}/#{bin}")) }
end

# --------------------------------------------------------------------------- #
# test_all
# --------------------------------------------------------------------------- #
desc "Test projects in pre-defined branches."
task :test_all do
    BRANCHES.each { |e| checkout(e) { Rake::Task[:test].execute }}
end

# --------------------------------------------------------------------------- #
# Copy
# --------------------------------------------------------------------------- #
desc "Copy umnamaged packages the bin directories."
task :copy, [:platform, :framework] => :restore do |_, e|
    v0 = (e.platform  != nil) ? [e.platform ] : PLATFORMS
    v1 = (e.framework != nil) ? [e.framework] : FRAMEWORKS

    v0.product(CONFIGS, v1) { |set|
        pf = (set[0] == 'Any CPU') ? 'x64' : set[0]
        COPIES.each { |key, value|
            src = FileList.new("#{LIB}/#{key}/runtimes/win-#{pf}/native/*.dll")
            value.each { |root|
                dest = "#{root}/bin/#{set[0]}/#{set[1]}/#{set[2]}"
                RakeFileUtils::mkdir_p(dest)
                RakeFileUtils::cp_r(src, dest)
            }
        }
    }
end

# --------------------------------------------------------------------------- #
# checkout
# --------------------------------------------------------------------------- #
def checkout(branch, &callback)
    sh("git checkout #{branch}")
    callback.call()
ensure
    sh("git checkout master")
end

# --------------------------------------------------------------------------- #
# cmd
# --------------------------------------------------------------------------- #
def cmd(args)
    sh("cmd.exe /c #{args}")
end
