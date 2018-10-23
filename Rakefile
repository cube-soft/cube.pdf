require 'rake'
require 'rake/clean'

# Configuration
SOLUTION = 'Cube.Pdf'
PROJECTS = [ 'Core', 'Ghostscript', 'Itext', 'Pdfium' ]
BRANCHES = [ 'master', 'net35' ]
COPY     = 'cp -pf'
CHECKOUT = 'git checkout'
BUILD    = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
RESTORE  = 'nuget restore'
PACK     = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'

# Tasks
task :default do
    Rake::Task[:clean].execute
    Rake::Task[:build].execute
    Rake::Task[:pack].execute
end

task :build do
    BRANCHES.each do |branch|
        sh("#{CHECKOUT} #{branch}")
        sh("#{RESTORE} #{SOLUTION}.sln")
        sh("#{BUILD} #{SOLUTION}.sln")
    end
end

task :pack do
    sh("#{CHECKOUT} net35")
    PROJECTS.each { |proj| sh("#{PACK} Libraries/#{proj}/#{SOLUTION}.#{proj}.nuspec") }
    sh("#{CHECKOUT} master")
end

PROJECTS.each { |proj| CLEAN.include("#{SOLUTION}.#{proj}.*.nupkg") }
