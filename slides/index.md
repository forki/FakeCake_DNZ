- title : FakeCake
- description : Fake, Cake and Build tools
- author : Παναγιώτης Καναβός
- theme : night
- transition : default


***
## Fake, Cake και Build tools

![FakeCake](images/cake.jpg)

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
### Στην παραλία 

![beach](images/beach.png)

Η λύση!


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

### Fake

![FakeLogo](images/fake.png)

Διαθέσιμο στο  [http://fsharp.github.io/FAKE/](http://fsharp.github.io/FAKE/)

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

    // include Fake lib
    #r @"packages/FAKE/tools/FakeLib.dll"
    open Fake

    // Default target
    Target "Default" (fun _ ->
        trace "Hello World from FAKE"
    )

    // start build
    RunTargetOrDefault "Default"


***

### Targets 

 * Τα βασικά βήματα
 * Καλούνται με όνομα
 * Συνδέονται μέσω dependencies


***

### Cleaning up

    let buildDir = "./build/"

    // Targets
    Target "Clean" (fun _ ->
        CleanDir buildDir
    )

    // Dependencies
    "Clean"
      ==> "Default"

*** 

### Dependencies 

* Τί πρέπει να τρέξει πρώτο, τί δεύτερο

<pre>
"Clean" 
  ==> "BuildApp" 
  ==> "Default"
</pre>

 * Conditional dependencies 

<pre>
"Clean" 
    ==> "BuildApp"
    =?> ("Test",hasBuildParam "xUnitTest")   
    ==> "Default"
</pre>

***

### Compiling the application

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

***

### File Sets

- Include files

    `!! "src/app/**/*.csproj"`

- Πρόσθετα αρχεία 

    `++ "test/**/*.csproj"`

* Εξαιρέσεις

    `-- "test/**/*.Integration.csproj"`

***

### Running tests

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

***

### Ένα πλήρες παράδειγμα

*** 

### Λείπει? Το φτιάχνεις

    //Returns 16.48.3.1511
    let myVersion =
        let dfi=DateTimeFormatInfo.CurrentInfo
        let calendar=dfi.Calendar

        let now=DateTime.Now
        let weekNum=calendar.GetWeekOfYear(now,dfi.CalendarWeekRule,dfi.FirstDayOfWeek)
        String.Format("{0:yy}.{1}.{2}.{0:HHmm}",now,weekNum,(int)now.DayOfWeek  )

***

### Builds from the Gurus

* PAKET - Τάξις!
    *  parseAllReleaseNotes !
* FAKE 
* FsReveal
    * Όλα σ' ένα!

*** 

### Και το PAKET ?

![PaketLogo](images/paket.png)

- Dependency Manager για .NET, Mono
- Δουλεύει με NuGet
- Συνεργάζεται με http, github repos 

Διαθέσιμο στο [https://fsprojects.github.io/Paket/](https://fsprojects.github.io/Paket/)

*** 

### Γιατί PAKET ?
- Επιτρέπει έλεγχο *όλων* των dependencies
- Διαχειρίζεται *transitive* dependencies?
' Έτυχε ποτέ ένα πακέτο να σας χαλάσει άλλο γιατί φόρτωσε άλλη έκδοση κάποιου τρίτου?
- Παρόμοιο θα ήταν και το project.json 

*** 

### Cake!

![CakeLogo](images/cake.png)

* Build tool σε C#
* Πιο πρόσφατο
* Πιο γνώριμο αλλά και πιο φλύαρο

Διαθέσιμο στο [http://cakebuild.net/](http://cakebuild.net/)

***

### Διαφορές σε ορολογία

* Targets --> tasks
* Dependencies στα tasks
* File patterns αντί για filesets
* **Και .NET Core!**

***

### Παράδειγμα  

    [lang="cs"]
    Task("Run-Unit-Tests")
        .IsDependentOn("Build")
        .Does(() =>
    {
        NUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
    });


***

### Packet/Tool Management

* Μέσω nuget
* Τροποποιήσεις στο bootstrapper

***

## Συμπεράσματα 

* Αυτοματοποίηση τώρα!
* Build script στη γλώσσα που προτιμάμε
* Dependency management και τέρμα οι χεράτες εγκαταστάσεις
* Και τα δύο δουλεύουν με TeamCity, AppVeyor, TFS κλπ


### FsReveal

- Δημιουργεί παρουσιάσεις με [reveal.js](http://lab.hakim.se/reveal-js/#/) από [markdown](http://daringfireball.net/projects/markdown/)
- Όλη η παρουσίαση ήταν ένα Markdown!
- Διαθέσιμο στο [http://fsprojects.github.io/FsReveal/](http://fsprojects.github.io/FsReveal/)

![FsReveal](images/logo.png)

***

### Reveal.js

- Framework για τη δημιουργία παρουσιάσεων σε JavaScript


> **Atwood's Law**: any application that can be written in JavaScript, will eventually be written in JavaScript.

***

### FSharp.Formatting

- F# tools for generating documentation (Markdown processor and F# code formatter).
- It parses markdown and F# script file and generates HTML or PDF.
- Code syntax highlighting support.
- It also evaluates your F# code and produce tooltips.

***

### Syntax Highlighting

#### F# (with tooltips)

    let a = 5
    let factorial x = [1..x] |> List.reduce (*)
    let c = factorial a

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
    recur_count k = 1 : 1 : 
        zipWith recurAdd (recur_count k) (tail (recur_count k))
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

---

### Paket

    [lang=paket]
    source https://nuget.org/api/v2

    nuget Castle.Windsor-log4net >= 3.2
    nuget NUnit
    
    github forki/FsUnit FsUnit.fs
      
---

### C/AL

    [lang=cal]
    PROCEDURE FizzBuzz(n : Integer) r_Text : Text[1024];
    VAR
      l_Text : Text[1024];
    BEGIN
      r_Text := '';
      l_Text := FORMAT(n);

      IF (n MOD 3 = 0) OR (STRPOS(l_Text,'3') > 0) THEN
        r_Text := 'Fizz';
      IF (n MOD 5 = 0) OR (STRPOS(l_Text,'5') > 0) THEN
        r_Text := r_Text + 'Buzz';
      IF r_Text = '' THEN
        r_Text := l_Text;
    END;

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

