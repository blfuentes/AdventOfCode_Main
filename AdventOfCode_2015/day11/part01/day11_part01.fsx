
let result = [1;2;3] |> List.pairwise(fun (a, b) -> [a] @ [b])



let increasingForThree(value: char list) : bool =
    [0..value.Length - 3]
    |> List.exists(fun i -> value.Item(i) = value.Item(i+1) && value.Item(i+1) = value.Item(i+2))

let notInvalidChars(value: char list) : bool =
    ((value |> Set.ofList) |> Set.intersect(Set.ofList(['i'; 'o'; 'l']))).Count = 0

let hasAtLeastTwoPairs(value: char list) : bool =
    (value |> List.pairwise |> List.filter(fun (a, b) -> a = b)).Length > 2

let isValidPassword(value: char list) : bool =
    increasingForThree(value) && notInvalidChars(value) && hasAtLeastTwoPairs(value)


let input = "abcdffaa".ToCharArray() |> List.ofArray
increasingForThree input
isValidPassword()