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
import SeriesDetails from '../../features/series/SeriesDetails';
import RootStore from '../stores/rootStore';
import UserProfile from '../../features/user/UserProfile';
import ArtistDetails from '../../features/artist/ArtistDetails';

const App = () => {

  const rootStore = useContext(RootStore);
  const {loadUser} = rootStore.userStore;

  useEffect(() => {
    loadUser()
  }, [])

  return(
    <Fragment>
        <ToastContainer position="bottom-right"/>
        <NavBar/>
        <Switch>
          <Route exact path="/" component={Homepage}/>
          <Route exact path="/series/:id" component={SeriesDetails} />
          <Route exact path="/users/:id" component={UserProfile} />
          <Route exact path="/artists/:id" component={ArtistDetails} />
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
