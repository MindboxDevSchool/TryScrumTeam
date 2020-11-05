import { useState } from "react";
import Login from './Login';
import Header from './Header';
import Body from './Body';


function App() {
  const hasToken = !!localStorage.getItem('token')
  const [isAuthenticated, setAuthenticated] = useState(hasToken);
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
          <Body />
        </>
        :
        <Login onLogin={authenticate} />
      }
    </div>
  );
}

export default App;
