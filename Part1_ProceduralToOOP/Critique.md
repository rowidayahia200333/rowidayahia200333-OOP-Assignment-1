# Procedural C++ Code Critique

1. Global bevaiors & Variables
that is bad because All application data (customers, products, orders) is stored in global variables and parallel arrays accessible from anywhere.
and  Any function can modify any part of the data at any time, making bugs hard to trace and state highly unpredictable.

 2. Lack of Encapsulation 
the arrays of customer , product and order not in their class they are belong not grouping in one class and any one can modifiy and see them that is not secure like data hiding

 3. Fixed Arrays 
 System limits are hardcoded (MAX_CUSTOMERS = 50, MAX_PRODUCTS = 50).
 The application cannot dynamically grow or handle data beyond these arbitrary boundaries.

