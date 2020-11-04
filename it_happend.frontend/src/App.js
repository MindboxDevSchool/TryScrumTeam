import { useState } from "react";
import './App.css';
import Login from './Login';
import Header from './Header';
import EventBox from './Components/Events/EventBox'
import EventList from './Components/Events/EventList'

function App() {
  const [isAuthenticated, setAuthenticated] = useState(false);
  const authenticate = (login, token) => {
    localStorage.setItem("token", token);
    localStorage.setItem("login", login);
    setAuthenticated(true);
  }
  const logOut = () => {
    localStorage.setItem("token", null);
    localStorage.setItem("login", null);
    setAuthenticated(false);
  }

  return (
    

    <div className="App">
      
      {isAuthenticated
        ?
        <>
          <Header onLogout={logOut} />
          <div>Body</div>
          <EventList trackName="vssdvsdvsd" AllowedCustomizations = {["scsc","cscsc"]} />
        </>
        :
        <Login onLogin={authenticate} />
      }
    </div>
  );
}

export default App;
