import { Menu, Container, Icon, Button, Segment, Label } from 'semantic-ui-react';
import React, { useContext, useEffect, Fragment } from 'react'
import {Link, NavLink, useHistory} from 'react-router-dom';
import UserStore from '../../app/stores/userStore';
import { AuthService } from '../../app/api/auth';
import { observer } from 'mobx-react-lite';
import { toast } from 'react-toastify';
import RootStore from '../../app/stores/rootStore';

const NavBar = () => {
    const authService = new AuthService();
    const rootStore = useContext(RootStore);
    const {userProfile, currentUser} = rootStore.userStore;
    let history = useHistory();


    const login = () =>  {
      authService.login().then(res => {},
      error => {
        toast.error("Error while logging in: " + error);
      })
      }

      const register = () => {
        history.push("/register")
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

    return (
      <Menu inverted>
      <Container>
        <Menu.Item header>
            <Icon name='tv' />
            Series Tracker
        </Menu.Item>
        <Menu.Item
          name="Series"
          as={NavLink}
          exact
          to='/series'
        />
        <Menu.Item
          name='Actors'
          as={NavLink}
          exact
          to='/actors'
        />

        {
          currentUser && userProfile && !currentUser.expired && (
            <Fragment>
              <Menu.Item position="right" header
                name={`Hi, ${userProfile?.name}`}
              />
              <Menu.Item>
                <Button onClick={logout}>
                  Logout
                </Button>
              </Menu.Item>
              <Menu.Item>
                <Button positive as={Link} to={`/users/${userProfile.userId}`}>
                  My profile
                </Button>
              </Menu.Item>
            </Fragment>
          )
        }

        {
          (!currentUser || currentUser.expired) && (
            <Fragment>
              <Menu.Item position="right">
                <Button color='green' onClick={register}>
                  Sign Up
                </Button>
              </Menu.Item>
              <Menu.Item>
                <Button onClick={login}>
                  Login
                </Button>
              </Menu.Item>
            </Fragment>
          )
        }


      </Container>
    </Menu>
    )
}
export default observer(NavBar)