package day07

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func concat(a, b int64) int64 {
	v, _ := strconv.ParseInt(strconv.FormatInt(a, 10)+strconv.FormatInt(b, 10), 10, 64)
	return v
}

func calculate2(expected, valueSoFar int64, eqparams []int64) bool {
	if valueSoFar > expected {
		return false
	}
	if len(eqparams) == 0 {
		return expected == valueSoFar
	}
	return calculate2(expected, valueSoFar+eqparams[0], eqparams[1:]) ||
		calculate2(expected, valueSoFar*eqparams[0], eqparams[1:]) ||
		calculate2(expected, concat(valueSoFar, eqparams[0]), eqparams[1:])
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day07/day07.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for index := 0; index < len(fileContent); index++ {
			expected := utilities.StringToInt64(strings.Split(fileContent[index], ":")[0])
			values := make([]int64, 0)
			for _, val := range strings.Split(strings.Split(fileContent[index], ":")[1], " ") {
				values = append(values, utilities.StringToInt64(val))
			}
			if calculate2(expected, 0, values) {
				result += expected
			}
		}
	}

	return result
}
