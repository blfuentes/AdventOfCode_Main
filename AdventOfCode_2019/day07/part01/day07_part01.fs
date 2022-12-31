module day07_part01

open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day07_input.txt"

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

let execute =
    snd permutation