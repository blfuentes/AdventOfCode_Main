open System.IO

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day07_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../../day05/day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"

let values = IntcodeComputerModule.getInputBigData filepath
let result0 = IntcodeComputerModule.execute(filepath, 5I)


let distrib e L =
    let rec aux pre post = 
        seq {
            match post with
            | [] -> yield (L @ [e])
            | h::t -> yield (List.rev pre @ [e] @ post)
                      yield! aux (h::pre) t 
        }
    aux [] L

let rec perms = function 
| [] -> Seq.singleton []
| h::t -> Seq.collect (distrib h) (perms t)

let permutation = 
    let results = 
        perms [0I .. 4I] |> Seq.map
            (fun perm -> 
                let result1 =  IntcodeComputerModule.executeWithPhase(filepath, (perm |> List.toArray).[0], 0I) 
                let result2 = IntcodeComputerModule.executeWithPhase(filepath, (perm |> List.toArray).[1], result1)
                let result3 = IntcodeComputerModule.executeWithPhase(filepath, (perm |> List.toArray).[2], result2)
                let result4 = IntcodeComputerModule.executeWithPhase(filepath, (perm |> List.toArray).[3], result3)
                let result5 = IntcodeComputerModule.executeWithPhase(filepath, (perm |> List.toArray).[4], result4)
                (perm, result5)
            ) |>Seq.maxBy snd
    results

// 4,3,2,1,0
let result1 =  IntcodeComputerModule.executeWithPhase(filepath, 4I, 0I) 
let result2 = IntcodeComputerModule.executeWithPhase(filepath, 3I, result1)
let result3 = IntcodeComputerModule.executeWithPhase(filepath, 2I, result2)
let result4 = IntcodeComputerModule.executeWithPhase(filepath, 1I, result3)
let result5 = IntcodeComputerModule.executeWithPhase(filepath, 0I, result4)

// 0,1,2,3,4
//let result1 =  IntcodeComputerModule.executeWithPhase(filepath, 0I, 0I) 
//let result2 = IntcodeComputerModule.executeWithPhase(filepath, 1I, result1)
//let result3 = IntcodeComputerModule.executeWithPhase(filepath, 2I, result2)
//let result4 = IntcodeComputerModule.executeWithPhase(filepath, 3I, result3)
//let result5 = IntcodeComputerModule.executeWithPhase(filepath, 4I, result4)

// 1,0,4,3,2
//let result1 =  IntcodeComputerModule.executeWithPhase(filepath, 1I, 0I) 
//let result2 = IntcodeComputerModule.executeWithPhase(filepath, 0I, result1)
//let result3 = IntcodeComputerModule.executeWithPhase(filepath, 4I, result2)
//let result4 = IntcodeComputerModule.executeWithPhase(filepath, 3I, result3)
//let result5 = IntcodeComputerModule.executeWithPhase(filepath, 2I, result4)