let rec lookAndSay (round: int) (max: int) (input: string) : string =
    printf "round: %A\n" round
    let rec lookAndSay' (consumed: string) (input: string list) : string =
        //printf "consumed: %A\n" consumed
        //printf "input: %A\n" input
        if input.IsEmpty then consumed
        else
            let initial = input |> List.takeWhile(fun x -> x = input.Head)
            //printfn "initial: %A" initial
            let remaining = input |> List.skipWhile(fun x -> x = input.Head)
            //printfn "remaining: %A" remaining
            let consumed' = initial.Length.ToString() + input.Head
            lookAndSay' (consumed + consumed') remaining
    if round = max then input
    else
        lookAndSay (round + 1) max (lookAndSay' "" (input.ToCharArray() |> Array.map string |> List.ofArray))

let result = lookAndSay 0 50 "1321131112"
printf "%i" result.Length


let read (input : string) =
    input
    |> Seq.fold (fun acc x ->
        match acc with
        | (n, x')::tl when x = x' -> (n+1, x')::tl
        | _ -> (1, x)::acc) []
    |> List.rev
    |> Seq.collect (fun (n, x) -> sprintf "%d%c" n x)
    |> fun xs -> System.String.Join("", xs)

//read "1321131112" |> printfn "result %s"
{1..50}
|> Seq.fold (fun acc _ -> read acc) "1321131112"
|> Seq.length