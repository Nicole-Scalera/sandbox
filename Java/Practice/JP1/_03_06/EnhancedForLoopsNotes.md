## Notes on the Enhanced For Loops

- For the control flow exercise, I am instructed to handle some enhanced for-loops.
- The author made note of some interesting parts.

### Fixed Size Collection
- Part 1 contains an Array, which is a fixed size collection.
    - Different from a List, which can change in size.

### Arrays.asList()
- Part 2 contains an ArrayList, which is similar in that it contains a collection of objects.
- The differences aren't as important here, but I found it particularly interesting how she wrote the line "Arrays.asList(...)"
- According to [GeeksForGeeks](https://www.geeksforgeeks.org/java/arrays-aslist-method-in-java-with-examples/) this method is used to convert an array into a fixed-size list.
- Another [article](https://medium.com/@AlexanderObregon/javas-arrays-aslist-method-explained-b308fac8f6fc) explains that the list returned can be modified via its existing elements.
- However, you cannot add or remove elements from the list.
- This is helpful for scenarios in which a read-only or fixed collection is needed.
- It looks like there's other benefits that can be explored that I should look into in the future.