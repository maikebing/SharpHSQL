use 14.0 MSBuild
use ".\packages\NUnit.ConsoleRunner\tools" nunit3-console

cls
task . Tests

task Restore {
    & .\.paket\paket.bootstrapper.exe
    & .\.paket\paket.exe restore
}

# Synopsis: Cleaning.
task Clean {
    if (Test-Path(".tests")) {  
        rm -r .tests/
    }
    
    rm mytest.*
    & git checkout src/SharpHSQL/SharpHSQL/doc
}

# Synopsis: Build the debug version.
task Build {
    exec { msbuild /v:q /nologo src\SharpHSQL\SharpHSQL.sln }
    # & "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}

# Synopsis: Building and running tests.
task Tests Clean, Restore, Build, {
    exec { nunit3-console src\SharpHSQL\SharpHSQL.UnitTests\bin\Debug\SharpHSQL.UnitTests.dll }
    exec { nunit3-console src\SharpHSQL\SharpHSQL.IntegrationTests\bin\Debug\SharpHSQL.IntegrationTests.dll }
}

# Synopsis: Building for Mono platform (using xbuild).
task Mono {
    & xbuild /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}

# Synopsis: Analysis of test coverage. Required dotCover.
task Cover Tests, {
    & tools\dotCover.2016.1\dotcover analyse tools\coverage.xml    
}