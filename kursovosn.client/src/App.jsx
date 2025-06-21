// App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import TournamentForm from './components/TournamentForm';
import TeamForm from './components/TeamForm';
import TournamentList from './components/TournamentList';
import TournamentDetails from './components/TournamentDetails';
import Navbar from './components/Navbar'
import Profile from './components/Profile'
import ChangePassword from './components/ChangePassword'
import { UserProvider } from './components/UserContext';
import EditProfile from './components/EditProfile';
import TournamentEntries from './components/TournamentEntries';
import TeamList from './components/TeamList';
import TeamDetails from './components/TeamDetails';
import TeamEntries from './components/TeamEntries';
import MatchPage from './components/MatchPage';
import MatchCreate from './components/CreateMatch';
import MatchEdit from './components/MatchEdit';
import MatchResultInput from './components/MatchResultInput';
import UserProfile from './components/UserProfile';
import BannedTournamentsPage from './components/BannedTournamentsPage';
import TeamEdit from './components/TeamEdit';
import EditTournament from './components/EditTournament';


function App() {
    return (
        <UserProvider>
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} /> {/* Главная страница */}
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/create-tournament" element={<TournamentForm />} />
                    <Route path="/list" element={<TournamentList />} />
                    <Route path="/create-team" element={<TeamForm />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/changePassword" element={<ChangePassword />} />
                    <Route path="/editProfile" element={<EditProfile />} />
                    <Route path="/tournaments/:id" element={<TournamentDetails />} />
                    <Route path="/tournaments/:id/Entries" element={<TournamentEntries />} />
                    <Route path="/team/:id" element={<TeamDetails />} />
                    <Route path="/team/:id/Entries" element={<TeamEntries />} />
                    <Route path="/Teamlist" element={<TeamList />} />
                    <Route path="/matches/:id" element={<MatchPage />} />
                    <Route path="/tournaments/:id/create-match" element={<MatchCreate />} />
                    <Route path="/matches/:id/Edit" element={<MatchEdit />} />
                    <Route path="/matches/:id/Result" element={<MatchResultInput />} />
                    <Route path="/profile/:userId" element={<UserProfile />} />
                    <Route path="/banTourn" element={<BannedTournamentsPage />} />
                    <Route path="/team/:id/edit" element={<TeamEdit />} />
                    <Route path="/tournaments/:id/edit" element={<EditTournament />} />
                </Routes>
            </Router>
        </UserProvider>
    );
}

function Home() {
    return (
        <div>
            <h1>Добро пожаловать на главную страницу!</h1>
            <nav>
                <ul>
                    <li>
                        <Link to="/login">Войти</Link>
                    </li>
                    <li>
                        <Link to="/register">Зарегистрироваться</Link>
                    </li>
                    <li>
                        <Link to="/create-tournament">Создать турнир</Link>
                    </li>
                    <li>
                        <Link to="/list">Список турниров</Link>
                    </li>
                    <li>
                        <Link to="/create-team">Создать команду</Link>
                    </li>
                    <li>
                        <Link to="/Teamlist">Список команд</Link>
                    </li>
                    <li>
                        <Link to="/profile">Профиль</Link>
                    </li>
                    <li>
                        <Link to="/banTourn">Забаненные турниры</Link>
                    </li>
                </ul>
            </nav>
        </div>
    );
}

export default App;

