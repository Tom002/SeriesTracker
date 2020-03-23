import React, { Fragment, useState } from 'react';
import { Header, Button, Label } from 'semantic-ui-react';
import { AuthService } from "../../app/api/auth";
import { toast, ToastContainer } from "react-toastify";
import Agent from '../../app/api/agents';
import { useHistory } from 'react-router-dom';

const Homepage = () => {

  var authService: AuthService = new AuthService();

  let history = useHistory();
  const [response, setResponse] = useState("");

  const login = () =>  {
    authService.login().then(res => {},
    error => {
      toast.error("Error while logging in: " + error);
    })
    }

  const logout = () => {
      authService.getUser().then(user => {
        if(user && !user.expired) {
          authService.logout().then(res => {},
          error => {
            toast.error("Error while logging out: " + error);
          })
        }
        else {
          toast.error("You are not logged in");
        }
      })
    }

  const userApiCall = async () => {
    authService.getUser().then(user => {
      if(user && !user.expired) {
        Agent.requests.get(`/users/${user.profile.sub}`).then(res => {
          setResponse(res);
        },
        error => {
          setResponse(error);
        })
      }
      else {
        toast.error("You are not logged in, or your token has expired");
      }
    })
  }

  const seriesApiCall = () => {
      Agent.requests.get("/series").then(res => {
        setResponse(res);
    }, error => {
      console.log("You are not logged in, or your token has expired");
      setResponse(error)
    })}


    const register = () => {
      history.push("/register")
  }


  return(
    <Fragment>
        <ToastContainer position="bottom-right"/>
        <Header as='h1'>
            SeriesTracker App
        </Header>
        <Button onClick={login}>
            Login 
        </Button>
        <Button onClick={logout}>
            Logout 
        </Button>
        <Button onClick={userApiCall}>
            GET User Data (Only accesible after login)
        </Button>
        <Button onClick={seriesApiCall}>
            Series GET
        </Button>
        <Button onClick={register}>
            Register
        </Button>
        <br></br>
        <br></br>
        <Label>
          {JSON.stringify(response)}
        </Label>
    </Fragment>

  )
}

export default Homepage