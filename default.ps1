Task default -depends "Build"

Task Build {
    & "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}

Task Restore {
    & .\.paket\paket.bootstrapper.exe
    & .\.paket\paket.exe restore
}

Task Tests {
    # TODO: Run tests
}

Task Mono {
    & xbuild /v:q /nologo src\SharpHSQL\SharpHSQL.sln
}