/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 1
	Description: This program implements and demonstrates the usage and functionality of a Fraction class. The
			Fraction class is made up of two integers (numerator and denominator). Fractions may be instantiated
			as 1/1, a whole number, a custom fraction, or as a pseudo-random fraction. Binary operations 
			(+, -, *, /) may be performed on Fractions. This program uses several cases to test the accuracy 
			of the Fraction class's implementation of these operations. The Fraction class lacks the
			ability to reduce fractions.
    Due Date: August 28, 2012
*/



#ifndef FRACTION_H
#define FRACTION_H



class Fraction {

	private:

		int num; // Fraction Numerator
		int den; // Fraction Denominator



	public:

		// Constructors
		Fraction();								  // Default    : 1/1
		Fraction(int numerator);				  // Overloaded : numerator/1
		Fraction(int numerator, int denominator); // Overloaded : numerator/denominator
		Fraction(bool random);                    // Overloaded : generates pseudo-random fraction


		// Destructor
		~Fraction(); 


		// Overloaded Operators 
		Fraction operator +(const Fraction& f2) const;
		Fraction operator -(const Fraction& f2) const;
		Fraction& operator =(const Fraction& source);

		friend Fraction operator *(const Fraction& f1, const Fraction& f2);
		friend Fraction operator /(const Fraction& f1, const Fraction& f2);
		friend std::ostream& operator <<(std::ostream& os, Fraction& source);
		friend std::istream& operator >>(std::istream& is, Fraction& source);


		// Misc.
		void print(std::ostream *os) const;


};



// Misc. helper functions
int getGCD(int a, int b);
int getLCM(int a, int b);



#endif
