// include Fake lib
#r "lib/FAKE/tools/FakeLib.dll"
open Fake

RestorePackages()

// Properties
let buildDir = "./build/"
let testDir  = "./test/"
let deployDir = "./deploy/"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "BuildApp" (fun _ ->
   !! "**/*.fsproj"
     |> MSBuildRelease buildDir "Build"
     |> Log "AppBuild-Output: "
)

Target "BuildAppDebug" (fun _ ->
   !! "**/*.fsproj"
     |> MSBuildDebug buildDir "Build"
     |> Log "AppBuild-Output: "
)

//Target "BuildTest" (fun _ ->
//    !! "src/test/**/*.csproj"
//      |> MSBuildDebug testDir "Build"
//      |> Log "TestBuild-Output: "
//)
//
//Target "Test" (fun _ ->
//    !! (testDir + "/NUnit.Test.*.dll")
//      |> NUnit (fun p ->
//          {p with
//             DisableShadowCopy = true;
//             OutputFile = testDir + "TestResults.xml" })
//)

//Target "Zip" (fun _ ->
//    !! (buildDir + "/**/*.*")
//        -- "*.zip"
//        |> Zip buildDir (deployDir + "Calculator." + version + ".zip")
//)

Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies
"Clean"
  ==> "BuildApp"
  //==> "BuildTest"
  //==> "Test"
  //==> "Zip"

"BuildAppDebug"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
