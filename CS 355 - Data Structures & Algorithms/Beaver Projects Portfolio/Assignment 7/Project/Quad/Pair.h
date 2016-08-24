#ifndef _PAIR_H
#define _PAIR_H

/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#include <iostream>
using namespace std;


class Pair {

	protected:

		int x;
		int y;

	public:

		Pair();
		Pair(int a, int b);

		void SetX(int);
		void SetY(int);
		void Set(int,int);
		int GetX()const;
		int GetY()const;

		virtual void Print(ostream &) const;

};


#endif