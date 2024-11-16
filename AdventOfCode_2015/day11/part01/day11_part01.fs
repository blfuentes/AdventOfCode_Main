module day11_part01

let increment (value: char list) : char list =
    let getNext (el: char) : (char*bool) =
        let value = (int)el + 1
        let nextchar = if value > 122 then (char)(value % 123 + 97) else (char)value
        (nextchar, value > 122)

    let result = 
        List.foldBack(fun el acc -> 
            let nextvalue = if (snd acc) > 0 then getNext el else (el, false)
            match nextvalue with
            | (next, up) when up = false -> 
                let r' = [next] @ (fst acc)
                (r', 0)
            | (next, up) when up = true -> 
                let r' =
                    if (fst acc).Length = value.Length - 1 then
                        'a' :: [next] @ (fst acc)
                    else
                        next :: (fst acc)
                (r', 1)
            | _ -> failwith "error"
        ) value ([], 1)

    fst result

let increasingForThree(value: char list) : bool =
    [0..value.Length - 3]
    |> List.forall(fun i -> value.Item(i) = value.Item(i+1) && value.Item(i+1) = value.Item(i+2))

let notInvalidChars(value: char list) : bool =
    ((value |> Set.ofList) |> Set.intersect(Set.ofList(['i'; 'o'; 'l']))).Count = 0

let hasAtLeastTwoPairs(value: char list) : bool =
    (value |> List.pairwise |> List.filter(fun (a, b) -> a = b)).Length > 2

let isValidPassword(value: char list) : bool =
    increasingForThree(value) && notInvalidChars(value) && hasAtLeastTwoPairs(value)

let execute =
    let input = "vzbxkghb"
    let test = increment ['x'; 'z']
    test.Length