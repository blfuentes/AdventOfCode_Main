module day02_part01

open System.IO

let rec performOperation (values: int[]) (idx: int)=
    if idx < values.Length then
        match values.[idx] with
            | 1 -> 
                Array.set values values.[idx + 3] (values.[values.[idx + 1]] + values.[values.[idx + 2]])
                performOperation values (idx + 4)
            | 2 -> 
                Array.set values values.[idx + 3] (values.[values.[idx + 1]] * values.[values.[idx + 2]])
                performOperation values (idx + 4)
            | 99 -> idx
            | _ -> idx
    else
       idx

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day02_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
    //let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_yavuz.txt"
    let values = File.ReadAllText(filepath).Split(',')
                    |> Array.map int
    
    // prepare the tranche input 
    Array.set values 1 12
    Array.set values 2 2

    let idx = performOperation values 0
        //printfn "first position in loop %d = %d" idx values.[0]
    //printfn "first position %d" values.[0]
    values.[0]