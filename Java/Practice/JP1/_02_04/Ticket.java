package _02_04;

public class Ticket {

  // Constructor for ticket
  public Ticket() {}

  private String destination; // Destination
  private Double price; // Price
  private boolean isReturn; // isReturn

  // Add three methods to set the value of each field, called setDestination,
  // setPrice and setIsReturn.

  public void setDestination(String destination) {
    this.destination = destination;
    // Left side: field
    // Right side: value that you're passing in
  }

  public void setPrice(double price) {
    this.price = price;
  }

  public void isReturn(boolean isReturn) {
    this.isReturn = isReturn;
  }

  // Add a separate method to get the value of each field, called getDestination,
  // getPrice and getIsReturn.

  public String getDestination() {
    return destination;
  }

  public double setPrice() {
    return price;
  }

  public boolean getIsReturn() {
    return isReturn;
  }

}
