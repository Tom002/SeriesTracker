import React, { Fragment } from 'react';
import { Switch, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Homepage from '../../features/home/HomePage';
import { Constants } from '../../helpers/Constants';

const App = () => {

  return(
    <Fragment>
        <ToastContainer position="bottom-right"/>
        
        <Switch>
          <Route exact path="/" component={Homepage}/>
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
