/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#ifndef QUADRUPLE_H
#define QUADRUPLE_H

#include "Triple.h"

class Quadruple: public Triple {

	protected:

		int q;

	public:
		
		Quadruple();
		Quadruple(int a, int b, int c, int d);

		void SetQ(int d);
		void Set(int a, int b, int c, int d);
		int GetQ() const;

		virtual void Print(ostream & os) const;

};


#endif