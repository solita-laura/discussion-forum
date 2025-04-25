import { Route, BrowserRouter, Routes, Navigate } from 'react-router-dom'
import '../App.css'
import LoginScreen from './screens/LoginScreen'
import Dashboard from './screens/Dashboard'
import TopicScreen from './screens/TopicScreen'

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Navigate to="login" />} path="/" />
        <Route path="/login" element={<LoginScreen/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
        <Route path="/topic" element={<TopicScreen />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
