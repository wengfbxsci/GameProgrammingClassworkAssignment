using System;
using System.Collections;
using System.Collections.Generic;

public class Cell
{
  //{left, right, top, bottom};
  //false is closed, true is open
  public bool[] sides = {false, false, false, false};
  public int rowIndex;
  public int colIndex;
  
  public Cell(int x, int y)
  {
    rowIndex = x;
    colIndex = y;
  }
  public void openSide(int sideIndex)
  {
    sides[sideIndex] = true;

  }
} 

class Maze
{
  private Random randy = new Random();
  private Stack<Cell> cellPath = new Stack<Cell>();
  private Cell[,] mazeCells;
  private int numOfRows;
  private int numOfCols;
  private int currRow; //current row index of generator
  private int currCol; //current col index of generator
  
  public Maze(int rowCount, int colCount)
  {
    //Initializes maze size and picks random location
    numOfRows = rowCount;
    numOfCols = colCount;
    mazeCells = new Cell[numOfRows, numOfCols];
    currRow = randy.Next(0, numOfRows);
    currCol = randy.Next(0, numOfCols);
    
    //Generates cell in maze and add onto stack
    mazeCells[currRow, currCol] = new Cell(currRow,currCol);
    cellPath.Push(mazeCells[currRow, currCol]);
  }
  
  void generateNextCell()
  {
    
    List<int> validDirections = new List<int>();
    
    //If there is a cell to the left
    if(currCol - 1 != -1 && mazeCells[currRow, currCol - 1] == null) //col - 1
        validDirections.Add(0);
    //If there is a cell to the right
    if(currCol + 1 < numOfRows && mazeCells[currRow, currCol + 1] == null) //col + 1
        validDirections.Add(1);
    //If there is a cell above
    if(currRow - 1 != -1 && mazeCells[currRow - 1, currCol] == null) //row - 1
      validDirections.Add(2);
    //If there is a cell below
    if(currRow + 1 < numOfCols && mazeCells[currRow + 1, currCol] == null) //row + 1
      validDirections.Add(3);
    
    //If a dead end is reached, start retracing steps
    if(validDirections.Count == 0){
      Cell poppedCell = cellPath.Pop();
      currRow = poppedCell.rowIndex;
      currCol = poppedCell.colIndex;
    }
    else
    { //Based on direction, change currRow & currCol
      switch(validDirections[randy.Next(validDirections.Count)])
      {
      case 0:
        mazeCells[currRow, currCol].openSide(0);
        cellPath.Push(mazeCells[currRow, currCol]);
        currCol--; //col - 1
        mazeCells[currRow, currCol] = new Cell(currRow, currCol);
        mazeCells[currRow, currCol].openSide(1);
        break;
      case 1:
        mazeCells[currRow, currCol].openSide(1);
        cellPath.Push(mazeCells[currRow, currCol]);
        currCol++;//col + 1
        mazeCells[currRow, currCol] = new Cell(currRow, currCol);
        mazeCells[currRow, currCol].openSide(0);
        break;
      case 2:
        mazeCells[currRow, currCol].openSide(2);
        cellPath.Push(mazeCells[currRow, currCol]);
        currRow--; //row - 1
        mazeCells[currRow, currCol] = new Cell(currRow, currCol);
        mazeCells[currRow, currCol].openSide(3);
        break;
      case 3:
        mazeCells[currRow, currCol].openSide(3);
        cellPath.Push(mazeCells[currRow, currCol]);
        currRow++; //row + 1
        mazeCells[currRow, currCol] = new Cell(currRow, currCol);
        mazeCells[currRow, currCol].openSide(2);
        break;
          
      default: 
        break;
  	  }
    }
    
    if(cellPath.Count != 0)
      generateNextCell();
  }
  
  public void generateMaze()
  {
    generateNextCell();
  }
  
  public override string ToString()
  {
    String mazeString = "+";
    for(int i = 0; i < numOfCols; i++){ //upper line
      mazeString += "--+";
    }
    
    mazeString += "\n|";
    
    for(int i = 0; i < numOfRows; i++){
      for(int j = 0; j < numOfCols; j++){
        mazeString += "  ";
        if(mazeCells[i,j].sides[1] == false){
          mazeString += "|"; //wall
        }
        else{
          mazeString += " "; //no wall
        }
      }
      
      mazeString+="\n+";
      
      for(int j = 0; j < numOfCols; j++){
        if(mazeCells[i,j].sides[3] == false){
          mazeString += "--"; //wall
        }
        else{
          mazeString += "  "; //no wall
        }
        mazeString += "+";
      }
      
      if(i < numOfRows - 1)
        mazeString +=" \n|";
      
    }
    return mazeString;
  }
  
}

class Controller
{
  static void returnIntInput(string errorMessage, out int number)
  {
    string input = Console.ReadLine();
    
    //Repeat error message if input is invalid, if input is valid, TryParse changes number
    while(!(Int32.TryParse(input, out number)))
    {
      Console.WriteLine(errorMessage);
      input = Console.ReadLine();
    }
  }
  
  static void Main()
  {
    int numOfRowsAndCols;
    Console.Write("Insert number of rows and columns: ");
    returnIntInput("Invalid input. Please enter an integer.", out numOfRowsAndCols);
    Console.WriteLine();
    
    //Creates empty maze
    
    Maze maze = new Maze(numOfRowsAndCols, numOfRowsAndCols);
    maze.generateMaze();
    Console.WriteLine(maze);
  }
}