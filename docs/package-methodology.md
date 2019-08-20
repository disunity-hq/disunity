# Package Methodology

## Overview
The Disunity Manager is responsible for downloading, installing, configuring and updating Disunity mod archives. This document outlines various related concerns and proposes a possible strategy for their implementation.

The overall methodology described here is inspired by the Nix package manager. 

Package archives are downloaded and extracted to a global flat filesystem store. The manager maintains a set of profiles for each target. A profile is a directory which can contain zero or more symlinks pointing to unpacked packages in the store. Every target always has a “default” profile which cannot be deleted. Every target has a single active profile which is a symlink to one of the target’s profiles. The loader is configured to locate mods within the target’s active profile directory. Changing the active profile is as easy as changing which profile it symlinks to.

##Package Identity

Each Disunity package is uniquely identified with three pieces of information:

Name of the Org that owns the mod
The name of the mod
A version string

##Package Storage

When downloading a package archive, the manager should immediately extract its contents to the global package store. The store is a single directory maintained by the manager where all extracted package archives are located. Each package archive should be extracted to a directory named with the following pattern:

    $manager-data-directory/store/$org-slug_$mod-slug_$version-string/

## Target Data

Within its data directory, the manager should maintain a data directory for each target. This data directory should contain things like the Disunity distribution for the target, along with it’s profiles and active profile.

## Profiles

For each target, the manager should maintain a directory containing a set of profiles. Each profile is a subdirectory. Each profile may contain 0 or more package symlinks. Profiles may not however contain symlinks to different versions of the same package. For each profile, the manager may want to maintain a small of metadata such as a name and description which may be provided by the user within the UI.

## Active Profile

For each target, the manager should maintain a special symlink called the “ActiveProfile”. This symlink points to one of the target’s profiles. When the profile to which the active profile points is renamed the active profile should also be updated. When the profile to which the active profile points is deleted, it should be set to the target’s “default” profile. Every target must always have a valid active profile.

The manager should configure the loaders within the Disunity distribution to look for mods within the target’s active profile. By changing which profile the active profile points to, the manager can change which mods are active for the target.

## Global Reference Index

The manager should maintain a global index of package references. For each package within the store, the manager should maintain a list of profiles which currently reference it. This has a number of advantages but there are three primary ones.

First, when a package has no referring profiles, that package may be deleted from the store in a process of garbage collection. This can help users save on disk space.

Second, when a package has at least one referring profile, we can challenge the user when they try to delete a package.

Third, we can validate the integrity of the manager’s overall state by validating that all packages within the index actually exist within the store. This can catch situations where a package is deleted from the store by some mechanism other than the manager itself.

As a bonus, when a user is looking at the details of a mod from within the manager, we can display a handy list of all the targets, and their profiles, which currently reference the package.

## Package Relations

Each package’s manifest may declare a number of dependencies and incompatibilities. Each dependency or incompatibility, we’ll call package relations, is specified in terms of:

Remote package identifier
Optional minimum version
Optional maximum version

### Dependency Resolution
For each dependency range, a concrete version must be resolved. Resolving a dependency range to a concrete version can be a challenge. The concrete version selected must be interoperable among the other selected concrete versions of a target’s dependencies. Furthermore, this must hold true recursively. 

Complicating matters, incompatible ranges are totalistically exclusive. This means no version within incompatibility ranges for a given package can be utilized for matching compatibility anywhere across all mod dependencies, globally.

### Mod Sorting
Once all mods, and their dependencies have been recursively resolved to concrete versions, they must be topologically sorted. If no sort is available, the manager must refuse to load the target with the offending profile.
