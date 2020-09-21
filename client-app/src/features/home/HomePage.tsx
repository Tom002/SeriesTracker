import React, { Fragment, useState, useContext } from 'react';
import { Header, Button, Label } from 'semantic-ui-react';
import { AuthService } from "../../app/api/auth";
import { toast, ToastContainer } from "react-toastify";
import Agent from '../../app/api/agent';
import { useHistory } from 'react-router-dom';
import SeriesStore from '../../app/stores/seriesStore';

const Homepage = () => {


  return(
    <Fragment>
        Home
    </Fragment>
  )
}

export default Homepage