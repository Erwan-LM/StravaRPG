# Strava RPG - Fitness Gamification App

## 1. Présentation du projet

Strava RPG est une application web de gamification fitness. L’utilisateur gagne de l’expérience (XP) et des niveaux en fonction de ses activités sportives, principalement récupérées via Strava.

L’objectif est de créer un système simple, évolutif et propre, permettant de transformer l’activité sportive en progression type RPG.

Le projet est personnel, construit en local au départ, sans budget, avec une architecture pensée pour évoluer vers une application plus complète si besoin.

---

## 2. Objectif fonctionnel

L’application permet à un utilisateur de :

- Se connecter via Google
- Connecter son compte Strava
- Synchroniser ses activités sportives
- Gagner de l’XP en fonction des kilomètres parcourus
- Monter en niveau automatiquement
- Suivre sa progression dans un dashboard

---

## 3. Stack technique

### Backend
- ASP.NET Core 8 Minimal API
- Entity Framework Core
- PostgreSQL (Docker local)

### Frontend
- Angular (standalone components)
- TailwindCSS
- Heroicons

### Authentification
- Google OAuth2
- Strava OAuth2

### Avatar
- DiceBear API

---

## 4. Architecture globale

Frontend Angular  
→ API ASP.NET Core  
→ Base de données PostgreSQL  
→ APIs externes :
- Google OAuth
- Strava API
- DiceBear

---

## 5. Règles de gameplay

### XP System

- 1 kilomètre = 1000 XP

Formule de progression des niveaux :

required_xp = 1000 + (level * 100)

Le niveau augmente automatiquement en fonction de l’XP total.

---

## 6. Modèle de données

### User

- id
- email
- name
- avatar_url
- xp
- level

### Activity

- id
- user_id
- source (strava, manual, future integrations)
- distance
- duration
- type
- date

### OAuthToken

- id
- user_id
- provider (google, strava)
- access_token
- refresh_token
- expires_at

---

## 7. Authentification

### Google OAuth
- Connexion principale
- Création automatique de l’utilisateur
- Génération de JWT

### Strava OAuth
- Connexion secondaire
- Accès aux activités sportives
- Stockage des tokens OAuth

---

## 8. Intégration Strava

- Authentification via OAuth2
- Récupération des activités utilisateur
- Synchronisation manuelle via bouton
- Stockage des activités en base
- Recalcul automatique de l’XP

---

## 9. Système XP

Un service dédié gère :

- Conversion des activités en XP
- Calcul du total XP utilisateur
- Détermination du niveau
- Protection contre les doublons

---

## 10. Interface utilisateur

### Pages

- Login
- Dashboard
- Profile

### Dashboard

- Avatar utilisateur (DiceBear)
- Niveau actuel
- Barre de progression XP
- Bouton de synchronisation Strava
- Liste des activités récentes

### Design

- Style SaaS moderne
- Inspiré de Notion, Linear et Strava
- TailwindCSS obligatoire
- Responsive mobile-first
- Interface minimaliste et lisible

---

## 11. Avatar system

- Génération via DiceBear API
- Basé sur email ou user id
- Utilisé dans toute l’application (header, profil, dashboard)

---

## 12. Backend architecture

Structure recommandée :

/Controllers  
/Services  
  - AuthService  
  - UserService  
  - StravaService  
  - XpService  
/Models  
/DTOs  
/Infrastructure  

---

## 13. Synchronisation des activités

- Synchronisation manuelle initialement
- Récupération des activités depuis la dernière synchronisation
- Détection des doublons
- Stockage en base
- Recalcul automatique de l’XP

---

## 14. Contraintes techniques

- Aucun framework inutile
- Pas de microservices
- Pas de Firebase
- Pas de Redux
- Code simple, lisible et maintenable
- Priorité au MVP fonctionnel

---

## 15. Étapes de développement

### Étape 1 - Base du projet
- Backend .NET fonctionnel
- Front Angular opérationnel
- PostgreSQL Docker configuré
- Communication API testée

### Étape 2 - Authentification
- Google OAuth
- Création utilisateur automatique
- JWT
- Dashboard protégé

### Étape 3 - XP System
- Ajout d’activités manuelles
- Calcul XP
- Gestion du niveau
- Affichage dashboard dynamique

### Étape 4 - Intégration Strava
- OAuth Strava
- Récupération activités
- Synchronisation
- Conversion XP automatique

### Étape 5 - UI/UX polish
- Interface SaaS propre
- UX fluide
- Responsive design
- Amélioration visuelle globale

---

## 16. Extensibilité future

Le système est conçu pour intégrer facilement :

- Apple Health
- Garmin
- Nike Run Club
- autres APIs sportives

Possibilités d’évolution :

- leaderboard global
- achievements
- statistiques avancées
- application mobile Flutter
- système de récompenses

---

## 17. Résumé

Le projet vise à créer une application simple mais évolutive permettant de transformer l’activité sportive en progression de type RPG.

L’objectif principal est de livrer un MVP fonctionnel rapidement, puis d’itérer progressivement vers une application plus complète et scalable.
