
# LTshowcase

A simple, lightweight project designed to showcase various products and services using ASP.NET Core Razor Pages. This project uses external APIs to dynamically pull data for product listings and individual product details, integrating Bootstrap for a responsive and modern UI.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [API Integration](#api-integration)
- [Testing](#testing)
## Overview

**LTshowcase** is a web application that demonstrates how to fetch, display, and interact with external product data via APIs. Built using ASP.NET Core Razor Pages and Visual Studio 2022, it implements the Vertical Slice Architecture, organizing code by features for a clear separation of concerns.

## Features

- List products with paging support.
- View individual product details on a separate page.
- Fetch data dynamically from external APIs.
- Responsive and clean UI using Bootstrap.
- Built with Vertical Slice Architecture to ensure each feature is self-contained and easy to maintain.

## Technologies Used

- **ASP.NET Core Razor Pages**: For building the web app.
- **Bootstrap**: For responsive design.
- **External API**: Uses [dummyjson.com](https://dummyjson.com) for fetching product data.
- **MediatR**: Used for implementing CQRS to keep the application decoupled and organized.
- **SliceFixture**: Provides integration testing capabilities for Vertical Slice Architecture, allowing you to test features end-to-end.
- **Visual Studio 2022**: Developed using the latest version of Visual Studio.

## Installation

Follow these steps to get the project up and running on your local machine:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/ben-ATX/LTshowcase.git
   ```

2. **Open the solution in Visual Studio 2022:**

- Open Visual Studio 2022.
- Click on File > Open > Project/Solution.
- Select the LTshowcase.sln file.

## Run the application:

- In Visual Studio, set the LTshowcase project as the startup project if it's not already.
- Restore NuGet packages manually if necessary.
- Press F5 or click on the green "Start" button to run the project.
- The application will launch in your default browser automatically.

## Usage
Once the application is running, you can:

- View the list of products fetched from the external API.
- Search for a product using a search term or product ID.
- Click on a product to view its details on a dedicated page.
- Use paging controls to navigate through larger product lists.
## API Integration
This project pulls product data from dummyjson.com. The API provides a list of products as well as individual product details.

Example API endpoints used in the project:
- Product List: https://dummyjson.com/products
- Filtered Product List: https://dummyjson.com/products/search?q=laptop
- Single Product: https://dummyjson.com/products/{id}
## Testing
The project includes a set of tests to ensure reliability and correctness. The tests are organized into classes based on different features and functionalities.

## Test Organization:
- **Search by Term and ID**: Tests that ensure the search functionality works both for searching by a term and finding a product by ID.
- **Paging Tests**: Covers scenarios where the product list needs to be paged due to a large number of results.
The test classes follow a structure where related functionalities (e.g., search and paging) are grouped together. This keeps the tests organized and ensures that all aspects of each feature are tested in a consolidated manner.
## Running Tests:
To run the tests in Visual Studio 2022:
- From the top menu, select **Test -> Run All Tests**
