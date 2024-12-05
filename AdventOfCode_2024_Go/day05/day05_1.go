package day05

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func IsInOrder(input []int, rules [][]int) bool {
	for i := 0; i < len(input)-1; i++ {
		if !utilities.Contains(rules[i], input[i]) {
			return false
		}
	}
	return true
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day05/day05.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		parts := strings.Split(fileContent, "")
		rules := make([][]int, 0)
		tobechecked := make([][]int, 0)
		for _, value := range strings.Split(parts[0], "\r\n") {
			prev, _ := strconv.Atoi(strings.Split(value, ":")[0])
			next, _ := strconv.Atoi(strings.Split(value, ":")[1])
			rules = append(rules, []int{prev, next})
		}
		for _, value := range strings.Split(parts[1], "\r\n") {
			serie := make([]int, 0)
			for _, value2 := range strings.Split(value, ",") {
				serie = append(serie, utilities.StringToInt(value2))
			}
			tobechecked = append(tobechecked, serie)
		}
		for cIdx := 0; cIdx < len(tobechecked); cIdx++ {

		}
	}

	return result
}
