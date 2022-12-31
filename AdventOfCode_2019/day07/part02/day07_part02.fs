module day07_part02

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
        perms [5I .. 9I] |> Seq.map 
            (fun perm -> 
                let values1 = IntcodeComputerModule.getInputBigData filepath
                let values2 = IntcodeComputerModule.getInputBigData filepath
                let values3 = IntcodeComputerModule.getInputBigData filepath
                let values4 = IntcodeComputerModule.getInputBigData filepath
                let values5 = IntcodeComputerModule.getInputBigData filepath

                let outputs= [|0I; 0I; 0I; 0I; 0I|]
                let indexes = [|0I; 0I; 0I; 0I; 0I|]
                let phases = (perm |> List.toArray)
                let runningAmp = [|true; true; true; true; true; |]

                let (output1, (index1, continue1), relBase1) =  IntcodeComputerModule.executeWithPhaseLoopMode(values1, phases.[0], indexes.[0], outputs.[0], 2I) 
                Array.set outputs 1 output1
                Array.set indexes 0 index1

                let (output2, (index2, continue2), relBase2) =  IntcodeComputerModule.executeWithPhaseLoopMode(values2, phases.[1], indexes.[1], outputs.[1], 2I) 
                Array.set outputs 2 output2
                Array.set indexes 1 index2

                let (output3, (index3, continue3), relBase3) =  IntcodeComputerModule.executeWithPhaseLoopMode(values3, phases.[2], indexes.[2], outputs.[2], 2I) 
                Array.set outputs 3 output3
                Array.set indexes 2 index3

                let (output4, (index4, continue4), relBase4) =  IntcodeComputerModule.executeWithPhaseLoopMode(values4, phases.[3], indexes.[3], outputs.[3], 2I) 
                Array.set outputs 4 output4
                Array.set indexes 3 index4

                let (output5, (index5, continue5), relBase5) =  IntcodeComputerModule.executeWithPhaseLoopMode(values5, phases.[4], indexes.[4], outputs.[4], 2I) 
                Array.set outputs 0 output5
                Array.set indexes 4 index5
                Array.set runningAmp 4 continue5

                Array.set phases 0 outputs.[0]
                Array.set phases 1 outputs.[1]
                Array.set phases 2 outputs.[2]
                Array.set phases 3 outputs.[3]
                Array.set phases 4 outputs.[4]

                let mutable result = 0

                while runningAmp.[4] do
                    let (output11, (index11, continue11), relBase11) =  IntcodeComputerModule.executeWithPhaseLoopMode(values1, outputs.[0], indexes.[0], outputs.[0], 1I) 
                    Array.set outputs 1 output11
                    Array.set indexes 0 index11
                    Array.set runningAmp 0 continue11

                    let (output22, (index22, continue22), relBase22) =  IntcodeComputerModule.executeWithPhaseLoopMode(values2, outputs.[1], indexes.[1], outputs.[1], 1I) 
                    Array.set outputs 2 output22
                    Array.set indexes 1 index22
                    Array.set runningAmp 1 continue22

                    let (output33, (index33, continue33), relBase33) =  IntcodeComputerModule.executeWithPhaseLoopMode(values3, outputs.[2], indexes.[2], outputs.[2], 1I) 
                    Array.set outputs 3 output33
                    Array.set indexes 2 index33
                    Array.set runningAmp 2 continue33

                    let (output44, (index44, continue44), relBase44) =  IntcodeComputerModule.executeWithPhaseLoopMode(values4, outputs.[3], indexes.[3], outputs.[3], 1I) 
                    Array.set outputs 4 output44
                    Array.set indexes 3 index44
                    Array.set runningAmp 3 continue44

                    let (output55, (index55, continue55), relBase55) =  IntcodeComputerModule.executeWithPhaseLoopMode(values5, outputs.[4], indexes.[4], outputs.[4], 1I) 
                    Array.set outputs 0 output55
                    Array.set indexes 4 index55
                    Array.set runningAmp 4 continue55                 
                (perm, outputs.[0])
            )
    let results2 = results |>Seq.maxBy snd
    results2

let execute =
    snd permutation