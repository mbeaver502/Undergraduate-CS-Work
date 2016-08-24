/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This program converts infix expressions to postfix expressions and vice versa.
	If the user specifies, the expression is evaluated after the conversion. Infix 
	expressions must be fully parenthesized, and postfix expressions must have
	appropriate spacing delineating operands and operators. For information
	on how to invoke this program, see the appropriate man page. WARNING: There is NO
	error checking to see if the expressions are entered correctly. They are assumed to 
	be correct upon being entered via the terminal. NB: This program is a substantial 
	expansion on a CS255 assignment for Dr. Jerkins from Fall 2011.
*/

#include <iostream>
#include <stack>
#include <string>
#include <stdlib.h>

using namespace std;

//--------------------------------------------------------------------------------------------

const int NUM_OPERATORS = 4; // Number of arithemetic operators
const int NUM_DIGITS = 10;   // 0..9

//--------------------------------------------------------------------------------------------

int parseArgs(char ** argv);

void convertToInfix(const string & exp, string & result);
void convertToPostfix(const string & exp, string & result);

bool isDigit(char item);
bool isOperator(char item);

float evalInfix(const string & exp);
float evalPostfix(const string & exp);
float calculate(float operand1, float operand2, char operation);

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Calls the appropriate sequence of functions to convert infix and postfix
		 expressions and evaluate them if specified.
	Incoming: argc is the number of arguments; argv is the list of arguments.
	Outgoing: N/A
	Return: Returns 0 on successful execution; otherwise, return 1.
*/
int main(int argc, char ** argv) {

	// We need at least 2 user-specified arguments
	if (argc <= 2) {

		cout << "Not enough arguments specified!" << endl;

		return 1;

	}

	cout << endl; // For somewhat pleasant spacing

	// Parse the arguments -- Return 1 if an error occurs
	if (!(parseArgs(argv)))
		return 1;

	return 0;

}

//--------------------------------------------------------------------------------------------

/*
	Name: parseArgs
	Author: Michael Beaver
	Purpose: Calls the appropriate functions to convert and evaluate the expressions as specified.
	Incoming: argv is the list of user-specified from the terminal.
	Outgoing: N/A
	Return: Return 0 on successful execution; otherwise, return 1.
*/
int parseArgs(char ** argv) {

	string result;
	string exp;
	int evalValue = 0;

	// Do not evaluate expression
	if (string(argv[2]) != "-e") {

		exp = argv[2];

		// Converting postfix to infix
		if (string(argv[1]) == "-i")
			convertToInfix(exp, result);

		// Converting infix to postfix
		else if (string(argv[1]) == "-o")
			convertToPostfix(exp, result);

		else {

			cout << "ERROR: Unrecognized flag '" << argv[1] << "'!" << endl;
			return 1;

		}
		
		cout << "Original expression: " << exp << endl << endl;
		cout << "   Infix expression: " << result << endl << endl;

	}

	// Evaluate expression, too
	else {

		exp = argv[3];

		// Converting postfix to infix
		if (string(argv[1]) == "-i")
			convertToInfix(exp, result);

		// Converting infix to postfix
		else if (string(argv[1]) == "-o")
			convertToPostfix(exp, result);

		else {

			cout << "ERROR: Unrecognized flag '" << argv[1] << "'!" << endl;
			return 1;

		}
		
		cout << "Original expression: " << exp << endl << endl;
		cout << "   Infix expression: " << result << endl << endl;
		cout << "    Evaluated value: " << evalInfix(result) << endl << endl;

	}

	return 0;

}

//--------------------------------------------------------------------------------------------

/*
	Name: convertToInfix
	Author: Michael Beaver
	Purpose: Convert a postfix expression to infix notation.
	Incoming: exp is the postfix expression, and result is the converted infix expression.
	Outgoing: result is the converted infix expression.
	Return: N/A (void)
*/
void convertToInfix(const string & exp, string & result) {

	stack<string> operands;

	int expLen = exp.length();
	int nextChar;

	string LHS;
	string RHS;
	string newLHS;

	result = "";

	// Loop through the expression and parse out the operands and operations
	for (int loopCount = 0; loopCount < expLen; loopCount++) {

		// If we haven't hit an operator...
		if (!(isOperator(exp[loopCount]))) {
                   
			// This string is for operands, especially multiple digit ones such as 123
			string operand; 
			nextChar = 1;
		           
			// If the current character is 0..9, check for a single or multiple digits
			if (isDigit(exp[loopCount])) {    
		                           
				operand = exp[loopCount];
		      
				// Checking to see if exp[loopCount + nextChar] is a digit, e.g. 123
				if (isDigit(exp[loopCount + nextChar])) {
		                        
					// Loop through to get all the digits
					while (isDigit(exp[loopCount + nextChar])) {
				    
						operand += exp[loopCount + nextChar];
						nextChar++;
				    
					}
		            
					// Add to the loop counter so it is after the operand
					loopCount += (operand.length() - 1);
		            
				}
		         
				// Push the mutiple digit operand onto the stack
				operands.push(operand);

			}

		}

		// If we have hit an operator, we need to adjust the stacks
		else if (isOperator(exp[loopCount])) {

			RHS = operands.top();
			operands.pop();

			LHS = operands.top();
			operands.pop();

			// Infix form: (LHS o RHS), where 'o' is the operation
			newLHS = "(" + LHS + " " + exp[loopCount] + " " + RHS + ")";

			// Push the new infix form onto the operands stack
			operands.push(newLHS);

		}

	}

	result = operands.top();
	operands.pop();

}

//--------------------------------------------------------------------------------------------

/*
	Name: convertToPostfix
	Author: Michael Beaver
	Purpose: Convert an infix expression to postfix notation.
	Incoming: exp is the infix expression, and result is the converted postfix expression.
	Outgoing: result is the converted postfix expression.
	Return: N/A (void)
*/
void convertToPostfix(const string & exp, string & result) {

	stack<char> operators;
	stack<string> operands;

	int expLen = exp.length();
	int nextChar;

	string LHS;
	string RHS;
	char op;

	result = "";

	// Loop through the expression and parse out the operands and operators
	for (int loopCount = 0; loopCount < expLen; loopCount++) {

		// We ignore open parens
		if (exp[loopCount] == '(')
			continue;
         
		// Keep looping if we haven't reached a closed paren
		else if (exp[loopCount] != ')') {
                   
			// This string is for operands, especially multiple digit ones such as 123
			string operand; 
			nextChar = 1;
		           
			// If the current character is 0..9, check for a single or multiple digits
			if (isDigit(exp[loopCount])) {    
		                           
				operand = exp[loopCount];
		      
				// Checking to see if exp[loopCount + nextChar] is a digit, e.g. 123
				if (isDigit(exp[loopCount + nextChar])) {
		                        
					// Loop through to get all the digits
					while (isDigit(exp[loopCount + nextChar])) {
				    
						operand += exp[loopCount + nextChar];
						nextChar++;
				    
					}
		            
					// Add to the loop counter so it is after the operand
					loopCount += (operand.length() - 1);
		            
				}
		         
				// Push the mutiple digit operand onto the stack
				operands.push(operand);

			}
            
			// Push the single-digit operand onto the stack
			else if (isOperator(exp[loopCount])) 
				operators.push(exp[loopCount]);
		
		}

		// When we hit a closed paren we have to update the operands stack
		else if (exp[loopCount] == ')') {

			RHS = operands.top();
			operands.pop();

			LHS = operands.top();
			operands.pop();

			op = operators.top();
			operators.pop();

			// Postfix form: LHS RHS o, where 'o' is the operator
			LHS += " " + RHS + " " + op;

			// Update the operands stack
			operands.push(LHS);

		}

	}

	result = operands.top();
	operands.pop();

}

//--------------------------------------------------------------------------------------------

/*
	Name: isDigit
	Author: Michael Beaver
	Purpose: Determines if a given character is a digit, [0..9].
	Incoming: item is the character in question.
	Outgoing: N/A
	Return: Returns true if item is a digit; returns false if item is not a digit.
*/
bool isDigit(char item) {
     
	char digits[NUM_DIGITS] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
     
	// Loop through the array to compare each value
	for (int i = 0; i < NUM_DIGITS; i++) {

		if (item == digits[i])
			return true;
            
	}
     
	return false;
     
}

//--------------------------------------------------------------------------------------------

/*
	Name: isOperator
	Author: Michael Beaver
	Purpose: Determines if a given character is an operator, [+, -, *, /].
	Incoming: item is the character in question.
	Outgoing: N/A
	Return: Returns true if item is an operator; returns false if item is not an operator.
*/
bool isOperator(char item) {
     
	char operators[NUM_OPERATORS] = {'+', '-', '*', '/'};
     
	// Loop through the array to compare each character
	for (int i = 0; i < NUM_OPERATORS; i++) {

		if (item == operators[i])
			return true;
            
	}
     
	return false; 
         
}

//--------------------------------------------------------------------------------------------

/*
	Name: evalInfix
	Author: Michael Beaver
	Purpose: Evaluates an infix expression by using stacks.
	Incoming: exp is the infix expression to be evaluated. 
	Outgoing: N/A
	Return: Returns the result of the evaluation as a float.
*/
float evalInfix(const string & exp) {

	stack<float> operands;
	stack<char> operators;
	
	float LHS;
	float RHS;
	char op;

	int expLen = exp.length();
	int nextChar;

	// Loop through the entire expression
	for (int loopCount = 0; loopCount < expLen; loopCount++) {
         
		// Keep looping if we haven't reached a closed paren
		if (exp[loopCount] != ')') {
                   
			// This string is for operands, especially multiple digit ones such as 123
			string operand; 
			nextChar = 1; 
		           
			// If the current character is 0..9, check for a single or multiple digits
			if (isDigit(exp[loopCount])) {    
		                           
				operand = exp[loopCount];
		      
				// Checking to see if exp[loopCount + nextChar] is a digit, e.g. 123
				if (isDigit(exp[loopCount + nextChar])) {
		                        
					// Loop through to get all the digits
					while (isDigit(exp[loopCount + nextChar])) {
		            
						operand += exp[loopCount + nextChar];
						nextChar++;
		            
					}
		            
					// Add to the loop counter so it is after the operand
					loopCount += (operand.length() - 1);
		            
				}
		         
				// Push the mutiple digit operand onto the stack
				operands.push(float(atoi(operand.c_str())));
			}
		    
			// Push the single-digit operand onto the stack
			else if (isOperator(exp[loopCount])) 
				operators.push(exp[loopCount]);
       
		} 
		 
		// When we hit a closed paren, we have to perform a calculation
		else if (exp[loopCount] == ')') {
               
			RHS = operands.top();
			operands.pop();

			LHS = operands.top();
			operands.pop();

			op = operators.top();
			operators.pop();

			// Calculate the operation and push back onto the operands stack
			operands.push(calculate(LHS, RHS, op));
                  
		}
                  
	}

	return operands.top();

}

//--------------------------------------------------------------------------------------------

/*
	Name: evalPostfix
	Author: Michael Beaver
	Purpose: Evaluates a postfix expression by using a stack.
	Incoming: exp is the postfix expression to be evaluated. 
	Outgoing: N/A
	Return: Returns the result of the evaluation as a float.
*/
float evalPostfix(const string & exp) {

	stack<float> operands;

	int expLen = exp.length();

	float LHS;
	float RHS;

	string operand;
	int nextChar;

	// Loop through the expression and parse out the operands and operators
	for (int loopCount = 0; loopCount < expLen; loopCount++) {

		// Collect the operand if present
		if (isDigit(exp[loopCount])) {

			operand = exp[loopCount];
			nextChar = 1;
		      
				// Checking to see if exp[loopCount + nextChar] is a digit, e.g. 123
				if (isDigit(exp[loopCount + nextChar])) {
		                        
					// Loop through to get all the digits
					while (isDigit(exp[loopCount + nextChar])) {
				    
						operand += exp[loopCount + nextChar];
						nextChar++;
				    
					}
		            
					// Add to the loop counter so it is after the operand
					loopCount += (operand.length() - 1);
		            
				}

			// Push the operand onto the stack
			operands.push(float(atoi(operand.c_str())));

		}

		// If we hit an operator, we have to perform a calculation
		else if (isOperator(exp[loopCount])) {

			RHS = operands.top();
			operands.pop();

			LHS = operands.top();
			operands.pop();

			// Perform the calculation and push the result onto the operands stack
			operands.push(calculate(LHS, RHS, exp[loopCount]));

		}

	}

	return operands.top();

}

//--------------------------------------------------------------------------------------------

/*
	Name: calculate
	Author: Michael Beaver
	Purpose: Performs a specified calculation on two float values.
	Incoming: operand1 is the LHS operand, operand2 is the RHS operand, and operation is
		  the operation to perform.
	Outgoing: N/A
	Return: Returns the result of the operation as a float.
*/
float calculate(float operand1, float operand2, char operation) {
    
	switch (operation) {
       
		// Addition              
		case '+': 
			return (operand1 + operand2);

		// Subtraction
		case '-': 
			return (operand1 - operand2);

		// Multiplication
		case '*': 
			return (operand1 * operand2);

		// Division
		case '/': {
             		
			// Make sure it's not dividing by 0
			if (operand2 != 0) 
				return (operand1 / operand2);
                
			else           
				cout << "ERROR: Division by zero!" << endl;

			break;

		}
             
		// An unknown error occurred, so we let them know, then return 0 at the end
		default:
			cout << "ERROR: An unknown error occurred!" << endl;
                                
	}
    
	return 0; 
    
}


