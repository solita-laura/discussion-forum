import { Route, BrowserRouter, Routes, Navigate } from 'react-router-dom'
import '../App.css'
import LoginScreen from './screens/LoginScreen'
import Dashboard from './screens/Dashboard'
import TopicScreen from './screens/TopicScreen'
import RegistrationScreen from './screens/RegistrationScreen'

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Navigate to="login" />} path="/" />
        <Route path="/login" element={<LoginScreen/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
        <Route path="/topic" element={<TopicScreen />} />
        <Route path="/registration" element={<RegistrationScreen/>} />
        <Route path="*" element={<LoginScreen/>} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
