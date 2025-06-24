package _03_02;

public class Main {

  public static void main(String[] args) {
    // Create a new instance of the GradingSystem
    GradingSystem gradingSystem = new GradingSystem();
    int percentage = 85; // Set the percentage grade here

    // These three lines simply print out information about the student's
    // grade (percentage, whether or not they passed, and the letter grade).
    System.out.println("Percentage: " + percentage);
    System.out.println("Pass: " + gradingSystem.isAPass(percentage));
    System.out.println("Grade: " + gradingSystem.getGrade(percentage));

    // The below line passes two parameters into the retakeMessage method in
    // GradingSystem.java, which influences whether or not they can retake the
    // exam, and will alter the output accordingly.
    System.out.println(gradingSystem.retakeMessage(percentage, true));
  }

}
