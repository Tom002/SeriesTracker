import React, { Fragment, useContext, useEffect } from 'react';
import { Switch, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Homepage from '../../features/home/HomePage';
import { Constants } from '../../helpers/Constants';
import NavBar from '../../features/nav/NavBar';
import './App.css';
import SeriesList from '../../features/series/SeriesList';
import UserStore from '../stores/userStore';
import ArtistList from '../../features/artist/ArtistList';

const App = () => {

  const userStore = useContext(UserStore);
  const {loadUser} = userStore;

  useEffect(() => {
    loadUser()
  }, [])

  return(
    <Fragment>
        <ToastContainer position="bottom-right"/>
        <NavBar/>
        <Switch>
          <Route exact path="/" component={Homepage}/>
          <Route exact path="/series" component={SeriesList} />
          <Route exact path="/actors" component={ArtistList} />
          <Route exact path="/register" 
            component={() => { 
              window.location.href = `${Constants.identityServer}account/register?ReturnUrl=${Constants.clientRoot}`; 
              return null;
         }}/>
        </Switch>
    </Fragment>

  )
}

export default App
