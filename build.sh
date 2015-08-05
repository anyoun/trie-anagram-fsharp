#!/bin/sh
mono nuget.exe install FAKE -ExcludeVersion -OutputDirectory lib -version 4.0.1
mono nuget.exe install ServiceStack -ExcludeVersion -OutputDirectory lib -Version 4.0.42
mono "lib/FAKE/tools/FAKE.exe" build.fsx BuildApp
