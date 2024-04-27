import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import BlogGeneration from "./components/BlogGeneration";
import BlogList from "./components/BlogList";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    },
    {
        path: '/generate-blog', // Route to generate a new blog
        element: <BlogGeneration />
    },
    {
        path: '/blog-list', // Route to list all generated blogs
        element: <BlogList />
    }
];

export default AppRoutes;
