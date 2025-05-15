import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import HomePage from './pages/HomePage'
import ConverterPage from './pages/ConverterPage'
import MemoryPage from './pages/MemoryPage'
import { createBrowserRouter, RouterProvider } from 'react-router'

const router = createBrowserRouter([
    { path: '/', element: <HomePage />},
    { path: '/converter', element: <ConverterPage />},
    { path: '/memory', element: <MemoryPage />}
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <RouterProvider router={router} />
    </StrictMode>,
)
