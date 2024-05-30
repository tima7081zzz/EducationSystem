import React, { useState } from 'react';
import { AppBar, Toolbar, IconButton, Typography, Drawer, List, ListItem, ListItemText, CssBaseline, Box, Button } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { Link } from 'react-router-dom';
import { logout } from '../../services/authService';

const drawerWidth = 240;

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [drawerOpen, setDrawerOpen] = useState(false);

  const handleDrawerToggle = () => {
    setDrawerOpen(!drawerOpen);
  };

  const handleLogout = async () => {
    try {
      await logout();
      window.location.href = '/login';
      
    } catch (error) {
      console.error('Failed to logout:', error);
      alert('Failed to logout. Please try again later.');
    }
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <IconButton edge="start" color="inherit" aria-label="menu" onClick={handleDrawerToggle} sx={{ mr: 2 }}>
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component={Link} to="/home">
            MyClass
          </Typography>
          <Box sx={{ flexGrow: 1 }} /> {/* Push the logout button to the right */}
          <Button color="inherit" onClick={handleLogout}>Logout</Button> {/* Logout button */}
        </Toolbar>
      </AppBar>
      <Drawer
        variant="persistent"
        anchor="left"
        open={drawerOpen}
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar />
        <Box sx={{ overflow: 'auto' }}>
          <List>
            <ListItem button component={Link} to="/home">
              <ListItemText primary="Home" />
            </ListItem>
            {/* Add the Settings menu item */}
            <ListItem button component={Link} to="/settings">
              <ListItemText primary="Settings" />
            </ListItem>
          </List>
        </Box>
      </Drawer>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3, transition: (theme) => theme.transitions.create('margin', {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen,
        }), marginLeft: drawerOpen ? `${drawerWidth}px` : 0 }}
      >
        <Toolbar />
        {children}
      </Box>
    </Box>
  );
};

export default Layout;
