package day01

import (
	"AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var fileName string = "./day01/day01.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		chars := []rune(fileContent)
	}

	return result
}
