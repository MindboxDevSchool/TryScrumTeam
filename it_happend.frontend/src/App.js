import { useState } from "react";
import './App.css';
import Login from './Login';
import Header from './Header';
import EventBox from './Components/Events/EventBox'

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
    <EventBox
CreatedAt ={new Date()}
Comment = "sfsfsf"
Rating = '8'
Scale = '100'
GeotagLatitude = '100'
GeotagLongitude = '100'
PhotoUrl = 'https://im0-tub-ru.yandex.net/i?id=62b749f07493402e841e8f66bb4570ff&ref=rim&n=33&w=391&h=188'
/>
    /*<div className="App">
      {isAuthenticated
        ?
        <>
          <Header onLogout={logOut} />
          <div>Body</div>
        </>
        :
        <Login onLogin={authenticate} />
      }
    </div>*/
  );
}

export default App;
