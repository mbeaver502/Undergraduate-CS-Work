/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#ifndef _TRIPLE_H
#define _TRIPLE_H

#include "Pair.h"

class Triple : public Pair {

	protected:

		int z;

	public:
	
		Triple();
		Triple(int, int, int);

		void SetZ(int);
		void Set(int, int, int);
		int GetZ()const;

		virtual void Print(ostream &) const;

};

#endif