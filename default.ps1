Task default -depends "Tests"

Task Restore {
    & .\.paket\paket.bootstrapper.exe
    & .\.paket\paket.exe restore
}

Task Clean {
    rm mytest.*    
}

Task Build -depends Restore {
    & "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}

Task Tests -depends Clean, Restore, Build {
    & packages\NUnit.ConsoleRunner\tools\nunit3-console.exe src\SharpHSQL\SharpHSQL.IntegrationTests\bin\Debug\SharpHSQL.IntegrationTests.dll
}

Task Mono {
    & xbuild /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}