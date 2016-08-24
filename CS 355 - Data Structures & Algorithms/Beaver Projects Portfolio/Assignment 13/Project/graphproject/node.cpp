/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#include <string>
#include "node.h"


//-------------------------------------------------------------
// NODE
//-------------------------------------------------------------


/*
  Name: Node in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Default constructor. Creates a Node with a valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::Node() {

	name = "unnamed";
	visited = false;
	changer = 0;
	totalWeight = INT_MAX;

}


/*
  Name: Node in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Overloaded constructor.  Allows the user to instatiate a valid Node object with an assigned name.
  Incoming: n is the user-defined name of the Node
  Outgoing: N/A
  Return: N/A
*/
Node::Node(string n) {

	name = n;
	visited = false;
	changer = 0;
	totalWeight = INT_MAX;

}


/*
  Name: Node in scope of Node
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Copy constructor.  Allows the user to instantiate a valid Node object from another node.
  Incoming: n is the source Node
  Outgoing: N/A
  Return: N/A
*/
Node::Node(const Node & n) {

	name = n.getName();  
	visited = n.hasBeenVisited();	      
	totalWeight = n.getTotalWeight();

	changer = n.getChanger();

	// Copy adjacencies
	adjList.clear();
	adjList.resize(n.getAdjList().size());
	for (unsigned int i = 0; i < adjList.size(); i++)
		adjList[i] = n.getAdjList()[i];	

}


/*
  Name: ~Node in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Destructor.  Returns memory and cleans up any data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::~Node() {

	name = "";	
	visited = false;
	totalWeight = 0;

	changer = 0;

	adjList.clear();  // Clean up the vector data member
	
}


/*
  Name: Overloaded assignment operator in scope of Node
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Allows the user to set a valid Node object and all of its properties 
		equal to another Node object and all of its properties.
  Incoming: n is the source Node
  Outgoing: N/A
  Return: Returns this Node with its new properties.
*/
Node & Node::operator =(const Node & n) {

	// Self-assignment
	if(this == &n)
		return *this;

	name = n.getName();  
	visited = n.hasBeenVisited();	      
	totalWeight = n.getTotalWeight();

	changer = n.getChanger();

	adjList.clear();
	adjList.resize(n.getAdjList().size());	
	for (unsigned int i = 0; i < adjList.size(); i++)
		adjList[i] = n.getAdjList()[i];	
	
	return *this;

}


/*
  Name: getName in scope of Node
  Writer(s): Michael Beaver
  Purpose: Returns the name of the Node
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the name of the Node
*/
string Node::getName() const {

	return name;

}


/*
  Name: wasVisited in scope of Node
  Writer(s): Michael Beaver
  Purpose: Returns the value stored in the visited boolean data member
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node has been visited; otherwise, returns false if the Node has not been visited
*/

bool Node::hasBeenVisited() const {

	return visited;

}



/*
  Name: setName in scope of Node
  Writer(s): Michael Beaver
  Purpose: Sets the name data member to a user-defined value.
  Incoming: n is the user-defined Node name
  Outgoing: N/A
  Return: N/A (void)
*/
void Node::setName(string n) {

	name = n;

}



/*
  Name: setVisitStatus in scope of Node
  Writer(s): Michael Beaver
  Purpose: Sets the visited status flag data member to a user-defined value.
  Incoming: flag is the user-defined status value.
  Outgoing: N/A
  Return: N/A (void)
*/
void Node::setVisitStatus(bool flag) {

	visited = flag;

}



/*
  Name: addAdjacentNode in scope of Node
  Writer(s): Michael Beaver
  Purpose: Adds an adjacent Node to a source Node's adjacency list.
  Incoming: adj is a pointer to the new adjacent Node, and edgeWeight is the weight of the Edge to be formed
  Outgoing: adj is a pointer to the new adjacent Node
  Return: N/A (void)
*/
void Node::addAdjacentNode(Node * adj, unsigned int edgeWeight) {

	// Create the new Edge from this Node to the adjacent Node
	Edge myNewEdge(this, adj, edgeWeight);

	// Add the new Edge to the adjacency list
	adjList.push_back(myNewEdge);

}


/*
  Name: displayAdjList in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Displays all the adjacent Nodes and Edges for this Node.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void Node::displayAdjList() const {

	int listSize = adjList.size();


	// No list, don't print
	if (listSize < 1)
		return;


	// Print all the adjacent Nodes and Edge weights
	// Src ---Weight---> Adj
	for (int i = 0; i < listSize; i++) {

		Edge e = adjList[i];
		cout << "[ " << name << " ] -- " << e.getWeight() << " --> [ " << e.getDst()->getName() << " ]" << endl;

	}

}


/*
  Name: hasAdjacentNodes in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Checks to see if a Node has adjacent Nodes or not.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node has adjacent nodes, false if it does not.
*/
bool Node::hasAdjacentNodes() const {

	if (adjList.size() > 0)
		return true;

	return false;

}


/*
  Name: getNumAdjacentNodes in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Allows another class to get the number of a Node's adjacent nodes.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of adjacent nodes that a Node has.
*/
int Node::getNumAdjacentNodes() const {

	return adjList.size();

}


/*
  Name: getAdjList in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Allows another class to get the adjacency list of a Node.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the adjacency list of a node.
*/
const vector<Edge> & Node::getAdjList() const{

	return adjList;

}


/*
  Name: setTotalWeight in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Allows an object outside of the Node class to set the total weight of a Node.
  Incoming: w is the integer in which to set this node's total weight as.
  Outgoing: N/A
  Return: N/A
*/
void Node::setTotalWeight(unsigned int w) {

	totalWeight = w;

}


/*
  Name: setChanger in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Allows an object outside of the Node class to set the changer of a Node.
  Incoming: n is a pointer to the node to be assigned as this node's changer.
  Outgoing: N/A
  Return: N/A
*/
void Node::setChanger(Node * n) {

	changer = n;

}


/*
  Name: getTotalWeight in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the total weight of a node.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the total weight of a node.
  */
int Node::getTotalWeight() const {

	return totalWeight;

}


/*
  Name: setVisited in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Allows an object outside of the Node class to flip the visited flag of a Node.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void Node::setVisited() {

	if(visited)
		visited = false;

	else
		visited = true;

}


/*
  Name: getChanger in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the changer of a node.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the changer of a node.
*/
Node * Node::getChanger() const {

	return changer;

}


/*
  Name: isNodeInAdjList in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: checks to see if a specified node is in this node's adjacency list
  Incoming: n is a pointer to the specified node.
  Outgoing: N/A
  Return: True if the specified node is in this node's adjacency list, false if it is not.
*/
bool Node::isNodeInAdjList(Node * n) const {

	int size = adjList.size();

	// Loop through the adjacency list
	for (int i = 0; i < size; i++) {

		// If the Node in question is a destination of any Edge, then return true
		if (adjList[i].getDst() == n)
			return true;

	}

	return false;

}


/*
  Name: getAdjNodeEdgeWeight in scope of Node
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the weight of the edge between the Node and the specified adjacent node.
  Incoming: adjNode is a pointer to a specified adjacent node
  Outgoing: N/A
  Return: Returns the weight of the edge between the Node and the specified adjacent node.
*/
unsigned int Node::getAdjNodeEdgeWeight(Node * adjNode) const {

	int size = adjList.size();

	// Loop through adjacency list
	for (int i = 0; i < size; i++) {

		// If the destination is the Node in question, then return the 
		// Edge weight associated with the adjacency
		if (adjList[i].getDst() == adjNode)
			return adjList[i].getWeight();

	}

	return 0;

}




//-------------------------------------------------------------
// EDGE
//-------------------------------------------------------------


/*
  Name: Edge in scope of Edge
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Default constructor.  Allows the user to create a basically null Edge.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Edge::Edge() {

	srcNode = 0;
	dstNode = 0;

	weight = 0;

}


/*
  Name: Edge in scope of Edge
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Overloaded constructor.  Allows the user to create an Edge with a given weight between two Nodes.
  Incoming: first is a pointer to the source Node, second is a pointer to the destination Node, and w is the 
			weight of the Edge
  Outgoing: first is a pointer to the source Node, and second is a pointer to the destination Node
  Return: N/A
*/
Edge::Edge(Node * first, Node * second, unsigned int w) {

	srcNode = first;
	dstNode = second;

	weight = w;

}


/*
  Name: Edge in scope of Edge
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Copy constructor.  Allows the user to instantiate a valid Edge object from another Edge object.
  Incoming: e is the source Edge
  Outgoing: N/A
  Return: N/A
*/
Edge::Edge(const Edge & e){

	srcNode = e.getSrc();
	dstNode = e.getDst();

	weight = e.getWeight();

}


/*
  Name: Overloaded assignment operator in scope of Edge
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Allows the user to set a valid Edge object and all of its properties 
		equal to another Edge object and all of its properties.
  Incoming: e is the source Edge
  Outgoing: N/A
  Return: Returns this Edge with its new properties.
*/
Edge & Edge::operator =(const Edge & e) {

	// Self-assignment
	if(this == &e)
		return *this;

	srcNode = e.getSrc();
	dstNode = e.getDst();

	weight = e.getWeight();

	return *this;
}


/*
  Name: ~Edge in scope of Edge
  Writer(s): Michael Beaver
  Purpose: Destructor.  Assigns pointers to NULL and resets Weight.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Edge::~Edge() {

	srcNode = 0;
	dstNode = 0;

	weight = 0;

}


/*
  Name: getSrc in scope of Edge
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns a pointer to the source Node of the Edge
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to the source Node of the Edge
*/
Node * Edge::getSrc() const {

	return srcNode;

}


/*
  Name: getDst in scope of Edge
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns a pointer to the adjacent Node of the Edge
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to the adjacent Node of the Edge
*/
Node * Edge::getDst() const {

	return dstNode;

}



/*
  Name: getWeight in scope of Edge
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the weight of the Edge
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the weight of the Edge
*/
unsigned int Edge::getWeight() const {

	return weight;

}


/*
  Name: setSrcNode in scope of Edge
  Writer(s): Michael Beaver
  Purpose: Sets the source Node of an Edge.  User may elect to delete the old source Node.
  Incoming: src is a pointer to the new source Node, and deleteNode is the flag that determines whether to
			delte the old Node first
  Outgoing: src is a pointer to the new source Node
  Return: N/A (void)
*/
void Edge::setSrcNode(Node * src, bool deleteNode) {

	// Return memory
	if (deleteNode)
		delete srcNode;

	srcNode = src;

}


/*
  Name: setDstNode in scope of Edge
  Writer(s): Michael Beaver
  Purpose: Sets the adjacent Node of an Edge.  User may elect to delete the old source Node.
  Incoming: dst is a pointer to the new adjacent Node, and deleteNode is the flag that determines whether to
			delte the old Node first
  Outgoing: dst is a pointer to the new destination Node
  Return: N/A (void)
*/
void Edge::setDstNode(Node * dst,  bool deleteNode) {

	// Return memory
	if (deleteNode)
		delete srcNode;

	dstNode = dst;

}


/*
  Name: setWeight in scope of Edge
  Writer(s): Michael Beaver
  Purpose: Sets the weight of an Edge.
  Incoming: w is the new weight of the Edge
  Outgoing: N/A
  Return: N/A (void)
*/
void Edge::setWeight(unsigned int w) {

	weight = w;

}
