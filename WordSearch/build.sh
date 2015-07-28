#!/bin/sh
mono nuget.exe install FAKE -OutputDirectory lib -ExcludeVersion -Prerelease
mono "lib/FAKE/tools/Fake.exe" build.fsx
