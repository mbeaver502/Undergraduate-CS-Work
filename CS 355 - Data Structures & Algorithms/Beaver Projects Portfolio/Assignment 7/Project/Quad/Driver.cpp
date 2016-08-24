/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#include "Quadruple.h"
#include <iostream>
#include <string>

using namespace std;
const string STARS = "****";

int main(){

	// Testing Constructors and Print
	Pair one;
	cout << STARS << endl;

	Pair two(1,3);
	cout << STARS << endl;

	one.Print(cout); cout << endl;
	cout << STARS << endl;
	two.Print(cout); cout << endl;	
	cout << STARS << endl;

	Triple hey;
	cout << STARS << endl;

	Triple four(10,20,30);
	cout << STARS << endl;

	hey.Print(cout); cout << endl;
	cout << STARS << endl;
	four.Print(cout); cout << endl;


	Quadruple five;
	Quadruple six(10, 20, 30, 40);

	cout << STARS << endl;

	five.Print(cout); cout << endl;
	cout << STARS << endl;

	six.Print(cout); cout << endl;
	cout << STARS << endl;

	five.Set(1, 2, 3, 4);
	five.Print(cout); cout << endl;
	cout << STARS << endl;

	cout << "Five's Q = " << five.GetQ() << endl;
	cout << STARS << endl;


	// Testing Pointers
	Pair * p1 = new Pair();
	Pair * p2 = new Pair(1,2);

	Triple * t1 = new Triple();
	Triple * t2 = new Triple(3,4,5);

	Quadruple * q1 = new Quadruple();
	Quadruple * q2 = new Quadruple(6,7,8,9);

	Pair * arr[6] = {p1, p2, t1, t2, q1, q2};


	for (int i = 0; i < 6; i++) {

		arr[i]->Print(cout);
		cout << endl << STARS << endl;


	}

	q2->Print(cout); cout << endl;


	delete p1;
	delete p2;

	delete t1;
	delete t2;

	delete q1;
	delete q2;
	

	system("pause");
	return 0;
}