#I @"Source\packages\Fake\tools"
#r "FakeLib.dll"

open Fake

(* properties *)
let authors = ["Max Malook"]
let projectName = "FeatureSwitcher.Examples"

TraceEnvironmentVariables()

let version = if isLocalBuild then getBuildParamOrDefault "version" "0.0.0.1" else buildVersion

(* Directories *)
let sourceDir = "./Source/"

let buildDir = "./Build/"
let deployDir = "./Release/"

(* files *)
let slnReferences = !! (sourceDir @@ "*.sln")

(* Targets *)
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; deployDir]
)

Target "BuildApp" (fun _ ->
    MSBuildRelease buildDir "Build" slnReferences
        |> Log "AppBuild-Output: "
)

Target "BuildZip" (fun _ ->
    !! (buildDir @@ "**/*.*")
        |> Zip buildDir (deployDir @@ sprintf "%s-%s.zip" projectName version)
)

Target "Default" DoNothing

// Build order
"Clean"
  ==> "BuildApp"
  ==> "BuildZip"
  ==> "Default"

// start build
RunParameterTargetOrDefault  "target" "Default"