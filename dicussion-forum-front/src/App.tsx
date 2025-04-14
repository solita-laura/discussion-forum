import { Route, BrowserRouter, Routes } from 'react-router-dom'
import '../App.css'
import LoginScreen from './screens/LoginScreen'
import Dashboard from './screens/Dashboard'
import TopicScreen from './screens/TopicScreen'

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginScreen/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
        <Route path="/topic" element={<TopicScreen />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
