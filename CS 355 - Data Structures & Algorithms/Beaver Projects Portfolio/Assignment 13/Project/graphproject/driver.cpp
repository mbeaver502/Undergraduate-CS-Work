/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#include <iostream>
#include <string>
#include "graph.h"

using namespace std;


void PrintMenu();


int main() {

	Graph g;
	string input;
	string output;
	string start;
	string finish;

	PrintMenu();
	cout << "-->";
	char choice;
	cin >> choice;
	
	Node * dst = 0;
	Node * src = 0;

	while(choice != 'q' && choice != 'Q') {

			switch (choice) {

				// Menu
				case 'h':
				case 'H': 
					PrintMenu(); 
					break;

				// Import from file
				case 'f':
				case 'F': {

					g.clearGraph();

					cout << endl << "Enter filename to use\n-->";
					cin >> input;
					cout << endl;

					cout << "Loading . . . ";

					g.readFromFile(input);

					cout << "Done." << endl;

					break;

				}

				// Find weighted shortest path
				case 'p':
				case 'P': {

					cin.clear();
					cin.ignore(INT_MAX, '\n');

					g.resetGraph();

					cout << endl << "Start Node: ";
					getline(cin, start);
					cout << "End Node: ";
					getline(cin, finish);
					cout << endl;

					Node * p = g.findNode(start);
					Node * q = g.findNode(finish);

					g.dijkstraHeap(p,q);

					break;

				}

				// Insert
				case '+': {
					
					string line;
					string srcNode;
					string dstNode;
					string weight;

					int pos;
					int pos2;

					getline(cin, line);

					cout << endl << "Inserting . . . ";

					// Get the source Node
					pos = line.find("->");

					// Only one Node
					if (pos == string::npos) {

						srcNode = line;

						if (g.findNode(srcNode) == 0) {

							src = new Node;
							src->setName(srcNode);

							g.addNewNode(src);

							cout << "Done." << endl;

						}

						else
							cout << "ERROR: Node already exists!" << endl;

						break;

					}

					for(int i = 0; i < pos; i++)
						srcNode += line[i];
					pos += 2;

					// Get the destination Node
					pos2 = line.find("(");

					if (pos2 == string::npos) {

						weight = "1";

						for (unsigned int i = pos; i < line.length(); i++)
							dstNode += line[i];	 

					}

					else {

						for(int i = pos; i < pos2; i++)
							dstNode += line[i];	 
						pos2++;
					

						// Get the Edge weight
						pos = line.find(")");
						for(int i = pos2; i < pos; i++) 
							weight += line[i];

					}

					// Find the Nodes in the Node array
					Node * p = g.findNode(srcNode);
					Node * q = g.findNode(dstNode);

					if (p == 0) {

						src = new Node;
						src->setName(srcNode);
						p = src;
						g.addNewNode(p);

					}

			
					if (q == 0) {

						dst = new Node;
						dst->setName(dstNode);
						q = dst;
						g.addNewNode(q);

					}

					// Complete the adjancency
					p->addAdjacentNode(q, atoi(weight.c_str()));

					srcNode = dstNode = weight = "";

					cout << "Done." << endl;
		 
					break;

				}

				case 'g':
				case 'G': {

					cout << endl;
					g.displayGraph();

					break;

				}

				case 'n':
				case 'N': {

					cout << endl;
					g.displayNodes();

					break;

				}

				case 's':
				case 'S': {

					cout << endl << "Enter name for save file\n-->";
					cin >> output;
					cout << endl;

					cout << "Saving . . . ";

					g.saveToFile(output);

					cout << "Done." << endl;

					break;

				}

				case 'r':
				case 'R': {
					
					string names;

					cin.clear();
					cin.ignore(INT_MAX, '\n');

					cout << endl << "Enter new name for distance units\n-->";
					getline(cin, names);
					cout << endl;

					g.setWeightUnits(names);

					cout << "Enter new name for vertex/node units\n-->";
					getline(cin, names);
					cout << endl;

					g.setNodeType(names);

					cout << "Enter new name for edge units\n-->";
					getline(cin, names);	  
					cout << endl;

					g.setEdgeType(names);

					break;

				}

				case 'c':
				case 'C': {

					cout << endl << "Clearing . . . ";

					g.clearGraph();

					cout << "Done." << endl;

					break;

				}

				default:
					cout << "INVALID CHOICE, Choose Again" << endl;

			}

		cout << endl;

		system("PAUSE");
		system("CLS");

		PrintMenu();
		cout << "--> ";
		cin >> choice;

	}


	g.~Graph();

	delete src;
	delete dst;

	cout << endl;
	system("PAUSE");
	return 0;

}


void PrintMenu() {

	cout << endl;
	cout << "\t------------------------" << endl;
	cout << "\t| Command Line Options |" << endl;
	cout << "\t------------------------" << endl << endl;

	cout << "  H\tHelp: Show Command Line Options" << endl << endl;

	cout << "  F\tImport From File" << endl;

	cout << "  +\tInsert Single Node or Adjacency" << endl;
	cout << "   \t\t> Single Node: +Node1" << endl;
	cout << "   \t\t> Adjacency:   +Node1->Node2         [weight = 1]" << endl;
	cout << "   \t\t> Adjacency:   +Node1->Node2(Weight) [no spaces]" << endl;
	cout << "  S\tSave Graph to File" << endl << endl;

	cout << "  P\tFind Weighted Shortest Path" << endl << endl;

	cout << "  G\tDisplay Graph" << endl;
	cout << "  N\tDisplay Nodes" << endl;
	cout << "  R\tRedefine Graph Units" << endl;
	cout << "  C\tClear Graph" << endl << endl;

    cout << "  Q\tQuit" << endl;
    cout << endl << endl;
	
}



