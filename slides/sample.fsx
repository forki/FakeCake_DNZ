﻿(**
- title : FakeCake
- description : Fake, Cake and Build tools
- author : Παναγιώτης Καναβός
- theme : night
- transition : default

***
# Fake, Cake και Build tools
### Παναγιώτης Καναβός

***

### Μία απλή υπόθεση

- Ένα console εργαλείο
- Μερικά tests
- Ένας Server
- Ένα deployment

***
### Με μερικές λεπτομέρειες

- Chocolatey για εύκολο upgrade
- Νέο version για κάθε deployment
* Δημιουργία package
* Αντιγραφή και upgrade

*** 
### Και λίγες ακόμα

- Αναβάθμιση NuGet packages
- Κατέβασμα εργαλείων όπως Chocolatey 

*** 
### Πως το κάνουμε?

* Με το χέρι
' Παίρνει πολύ ώρα, βαρετό, πολύ εύκολο λάθος
* Με build events!
' Edit σε 3 γραμμές? DOS scripting?

***
#### Με ένα build server

* Τeam City, TFS
* Εγκατάσταση server, agents, ρυθμίσεις, βάσεις
* Ποιός βάζει το chocolatey?
* Πώς φτιάχνουμε το νέο version number?
* Πως κάνω τοπικά build?

***
#### Με build tools σε XML

* MSBuild (XML)
* NAnt (XML)

Μα δεν προγραμματίζω σε XML! Πάλι θα κυνηγάω addins?

***
#### Με buid tools στη γλώσσα μου
* PSake (Powershell)
* FAKE (F#)
* Cake (C#)

***
#### Γιατί Fake και Cake ?

<table>
<th></th><th>Downloads</th><th>Commits</th><th>Contributors</th>
<tr><td>PSake </td><td> 132K </td><td>407 </td><td>48 </td>
<tr><td>FAKE  </td><td> 635K </td><td>6000 </td><td>244 </td>
<tr><td>Cake  </td><td> 210K </td><td>1654</td><td>94</td>
</table>

***
#### Τί είναι?

- "DSL" για build tasks σε F# ή C#
- Βασισμένα σε F# ή C# scripting
- Δεκάδες έτοιμα tasks (Git, Unit Testing, Azure etc)
- Λείπει κάτι? Το γράφεις επί τόπου 

***

### Τρέχοντας το FAKE

Με αυτόματη εγκατάσταση μέσω NuGet

    @echo off
    cls
    .nuget/NuGet.exe Install FAKE -ExcludeVersion
    packages/FAKE/tools/Fake.exe build.fsx
    pause

ή με PAKET

    @echo off
    cls
    .paket/paket.exe restore
    packages/FAKE/tools/Fake.exe build.fsx
    pause

***

### Hello world
*)
    // include Fake lib
    #r @"packages/FAKE/tools/FakeLib.dll"
    open Fake

    // Default target
    Target "Default" (fun _ ->
        trace "Hello World from FAKE"
    )

    // start build
    RunTargetOrDefault "Default"

(**

***

### Cleaning up

*)
    let buildDir = "./build/"

    // Targets
    Target "Clean" (fun _ ->
        CleanDir buildDir
    )

    // Dependencies
    "Clean"
      ==> "Default"
(**

***

### Compiling the application

*)

    Target "BuildApp" (fun _ ->
        !! "src/app/**/*.csproj"
        ++ "src/app**/*.fsproj"
          |> MSBuildRelease buildDir "Build"
          |> Log "AppBuild-Output: "
    )

    // Dependencies
    "Clean"
      ==> "BuildApp"
      ==> "Default"

(**

***

### Compiling the application

*)

    Target "BuildApp" (fun _ ->
        !! "src/app/**/*.csproj"
          |> MSBuildRelease buildDir "Build"
          |> Log "AppBuild-Output: "
    )

    // Dependencies
    "Clean"
      ==> "BuildApp"
      ==> "Default"

*)

***

### Running tests

(**
    Target "Test" (fun _ ->
        !! (testDir </> "NUnit.Test.*.dll")
          |> NUnit (fun p ->
              {p with
                 // override default parameters
                 DisableShadowCopy = true;
                 OutputFile = testDir </> "TestResults.xml" })
    )


    "Clean"
      ==> "BuildApp"
      ==> "BuildTest"  
      ==> "Test"
      ==> "Default"

*)

***

### Ένα πλήρες παράδειγμα

*** 

### Και το PAKET ?

- Dependency Manager για .NET, Mono
- Δουλεύει με NuGet
- Συνεργάζεται με http, github repos 

*** 

### Γιατί PAKET ?
- Επιτρέπει έλεγχο *όλων* των dependencies
- Διαχειρίζεται *transitive* dependencies?
' Έτυχε ποτέ ένα πακέτο να σας χαλάσει άλλο γιατί φόρτωσε άλλη έκδοση κάποιου τρίτου?
- Παρόμοιο θα ήταν και το project.json 

*)
let a = 5
let factorial x = [1..x] |> List.reduce (*)
let c = factorial a
(** 
`c` is evaluated for you
*)
(*** include-value: c ***)
(**

--- 

#### More F#

*)
[<Measure>] type sqft
[<Measure>] type dollar
let sizes = [|1700<sqft>;2100<sqft>;1900<sqft>;1300<sqft>|]
let prices = [|53000<dollar>;44000<dollar>;59000<dollar>;82000<dollar>|] 
(**

#### `prices.[0]/sizes.[0]`

*)
(*** include-value: prices.[0]/sizes.[0] ***)
(**

---

#### C#

    [lang=cs]
    using System;


    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, world!");
        }
    }


---

#### JavaScript

    [lang=js]
    function copyWithEvaluation(iElem, elem) {
      return function (obj) {
          var newObj = {};
          for (var p in obj) {
              var v = obj[p];
              if (typeof v === "function") {
                  v = v(iElem, elem);
              }
              newObj[p] = v;
          }
          if (!newObj.exactTiming) {
              newObj.delay += exports._libraryDelay;
          }
          return newObj;
      };
    }

---

#### Haskell
 
    [lang=haskell]
    recur_count k = 1 : 1 : zipWith recurAdd (recur_count k) (tail (recur_count k))
            where recurAdd x y = k * x + y

    main = do
      argv <- getArgs
      inputFile <- openFile (head argv) ReadMode
      line <- hGetLine inputFile
      let [n,k] = map read (words line)
      printf "%d\n" ((recur_count k) !! (n-1))


*code from [NashFP/rosalind](https://github.com/NashFP/rosalind/blob/master/mark_wutka%2Bhaskell/FIB/fib_ziplist.hs)*

---

### SQL
 
    [lang=sql]
    select * 
    from 
      (select 1 as Id union all select 2 union all select 3) as X 
    where Id in (@Ids1, @Ids2, @Ids3)

*sql from [Dapper](https://code.google.com/p/dapper-dot-net/)* 

***

**Bayes' Rule in LaTeX**

$ \Pr(A|B)=\frac{\Pr(B|A)\Pr(A)}{\Pr(B|A)\Pr(A)+\Pr(B|\neg A)\Pr(\neg A)} $

***

### The Reality of a Developer's Life 

**When I show my boss that I've fixed a bug:**
  
![When I show my boss that I've fixed a bug](http://www.topito.com/wp-content/uploads/2013/01/code-07.gif)
  
**When your regular expression returns what you expect:**
  
![When your regular expression returns what you expect](http://www.topito.com/wp-content/uploads/2013/01/code-03.gif)
  
*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*

*)